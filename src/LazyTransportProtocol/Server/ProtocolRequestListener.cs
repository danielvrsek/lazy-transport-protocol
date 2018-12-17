using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Responses.Abstractions;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Server.Protocol;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Application.Transport.Model;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace LazyTransportProtocol.Server
{
	public class ProtocolRequestListener
	{
		private static ProtocolRequestExecutor executor = new ProtocolRequestExecutor();
		private static TransportLayer transportLayer = new TransportLayer();
		private static AvailableRequestsContainer _availableRequests;

		private static MethodInfo _executeMethod;

		public void Listen(IPAddress ipAddress, int port)
		{
			Console.WriteLine("Initializing...");
			SetExecuteMethod();
			Console.WriteLine("Registering available requests...");
			RegisterRequests();
			Console.WriteLine("Waiting for connection..");
			transportLayer.Listen(ipAddress, port, OnClientConnected, OnDataReceived, OnErrorOccured);
		}

		private void RegisterRequests()
		{
			_availableRequests = new AvailableRequestsContainer();
			_availableRequests.RegisterByType<IProtocolRequest<IProtocolResponse>>(req => req.GetIdentifier());

			string requests = String.Join(", ", _availableRequests.GetAll().Select(x => x.Key));
			Console.WriteLine("Available requests: ");
			Console.WriteLine(requests);
		}

		private void SetExecuteMethod()
		{
			_executeMethod = typeof(ProtocolRequestListener).GetMethod(nameof(Execute), BindingFlags.NonPublic | BindingFlags.Static);
		}

		public static void OnClientConnected(IClientConnection connection)
		{
			// NOOP
		}

		public static void OnDataReceived(IClientConnection connection, byte[] data)
		{
			try
			{
				ProtocolEncoder encoder = new ProtocolEncoder();

				MediumDeserializedObject requestObject = null;
				try
				{
					requestObject = ProtocolSerializer.Deserialize(data);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				string controlCommand = encoder.Decode(requestObject.ControlCommand);

				IList<ArraySegment<byte>> serializedResponse = null;
				Type requestType = _availableRequests.GetRequestType(controlCommand);
				if (requestType != null)
				{
					try
					{
						Type protocolRequestInterface = requestType.GetInterfaces().FirstOrDefault(x => typeof(IProtocolRequest<IProtocolResponse>).IsAssignableFrom(x));
						Type responseType = protocolRequestInterface.GenericTypeArguments[0];
						serializedResponse = (IList<ArraySegment<byte>>)_executeMethod.MakeGenericMethod(requestType, responseType).Invoke(null, new object[] { requestObject.Body });
					}
					catch
					{
						serializedResponse = SerializeResponse(new ErrorResponse
						{
							Code = 500,
							Message = "Internal server error."
						});
					}
				}
				else
				{
					serializedResponse = SerializeResponse(new ErrorResponse
					{
						Code = 400,
						Message = "Unsupported request."
					});
				}

				connection.Send(serializedResponse);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static IList<ArraySegment<byte>> Execute<TRequest, TResponse>(ArraySegment<byte> body)
			where TRequest : IProtocolRequest<TResponse>
			where TResponse : class, IProtocolResponse, new()
		{
			IList<ArraySegment<byte>> serializedResponse = null;

			try
			{
				TRequest request = ProtocolBodySerializer.Deserialize<TRequest>(body);
				serializedResponse = SerializeResponse(executor.Execute(request));
			}
			catch (AuthorizationException e)
			{
				serializedResponse = SerializeResponse(new ErrorResponse
				{
					Code = 403,
					Message = "Unauthorized."
				});

				Console.WriteLine("AuthorizationException: " + e.Message + e.StackTrace);
			}
			catch (CustomException e)
			{
				serializedResponse = SerializeResponse(new ErrorResponse
				{
					Code = 400,
					Message = e.Message
				});

				Console.WriteLine("CustomException: " + e.Message + e.StackTrace);
			}
			catch (Exception e)
			{
				serializedResponse = SerializeResponse(new ErrorResponse
				{
					Code = 500,
					Message = "Internal server error."
				});

				Console.WriteLine("UnhandledException: " + e.Message + e.StackTrace);
			}

			return serializedResponse;
		}

		private static IList<ArraySegment<byte>> SerializeResponse<TResponse>(TResponse protocolResponse)
			where TResponse : IProtocolResponse, new()
		{
			ProtocolEncoder encoder = new ProtocolEncoder();

			MessageHeadersDictionary responseHeaders = new MessageHeadersDictionary();

			byte[] identifier = encoder.Encode(protocolResponse.GetIdentifier());
			byte[] headers = ProtocolMessageHeaderSerializer.Serialize(responseHeaders);
			byte[] serializedBody = ProtocolBodySerializer.Serialize(protocolResponse);

			return ProtocolSerializer.Serialize(identifier, headers, serializedBody);
		}

		private static void OnErrorOccured(ErrorContext ctx)
		{
			string message = ctx?.Exception?.Message ?? "Unidentified error occured.";

			Console.WriteLine(message);
		}
	}
}