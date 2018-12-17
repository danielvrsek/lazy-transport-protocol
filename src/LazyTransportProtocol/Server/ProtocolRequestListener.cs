using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
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
using System.Net;

namespace LazyTransportProtocol.Server
{
	public class ProtocolRequestListener
	{
		private static ProtocolRequestExecutor executor = new ProtocolRequestExecutor();
		private static TransportLayer transportLayer = new TransportLayer();

		public void Listen(IPAddress ipAddress, int port)
		{
			Console.WriteLine("Waiting for connection..");
			transportLayer.Listen(ipAddress, port, OnClientConnected, OnDataReceived, OnErrorOccured);
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

				ArraySegment<byte> serializedResponse = null;

				string controlCommand = encoder.Decode(requestObject.ControlCommand);

				switch (controlCommand)
				{
					case CreateUserRequest.Identifier:
						serializedResponse = Execute<CreateUserRequest, AcknowledgementResponse>(requestObject.Body);
						break;

					case DeleteUserRequest.Identifier:
						serializedResponse = Execute<DeleteUserRequest, AcknowledgementResponse>(requestObject.Body);
						break;

					case AuthenticationRequest.Identifier:
						serializedResponse = Execute<AuthenticationRequest, AuthenticationResponse>(requestObject.Body);
						break;

					case ListDirectoryClientRequest.Identifier:
						serializedResponse = Execute<ListDirectoryClientRequest, ListDirectoryResponse>(requestObject.Body);
						break;

					case DownloadFileRequest.Identifier:
						serializedResponse = Execute<DownloadFileRequest, DownloadFileResponse>(requestObject.Body);
						break;

					case CreateDirectoryRequest.Identifier:
						serializedResponse = Execute<CreateDirectoryRequest, AcknowledgementResponse>(requestObject.Body);
						break;

					case UploadFileRequest.Identifier:
						serializedResponse = Execute<UploadFileRequest, AcknowledgementResponse>(requestObject.Body);
						break;

					default:
						serializedResponse = SerializeResponse(new ErrorResponse
						{
							Code = 400,
							Message = "Unsupported request."
						});
						break;
				}

				connection.Send(serializedResponse);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static ArraySegment<byte> Execute<TRequest, TResponse>(ArraySegment<byte> body)
			where TRequest : IProtocolRequest<TResponse>
			where TResponse : class, IProtocolResponse, new()
		{
			ArraySegment<byte> serializedResponse = null;

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

		private static ArraySegment<byte> SerializeResponse<TResponse>(TResponse protocolResponse)
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