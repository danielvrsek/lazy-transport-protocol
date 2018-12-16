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

				string requestString = encoder.Decode(data);

				MediumDeserializedObject requestObject = null;
				try
				{
					requestObject = ProtocolSerializer.Deserialize(requestString);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				IProtocolResponse protocolResponse = null;
				IProtocolRequest<IProtocolResponse> request = null;

				switch (requestObject.ControlCommand)
				{
					case CreateUserRequest.Identifier:
						Deserialize<CreateUserRequest>();
						break;

					case DeleteUserRequest.Identifier:
						Deserialize<DeleteUserRequest>();
						break;

					case AuthenticationRequest.Identifier:
						Deserialize<AuthenticationRequest>();
						break;

					case ListDirectoryClientRequest.Identifier:
						Deserialize<ListDirectoryClientRequest>();
						break;

					case DownloadFileRequest.Identifier:
						Deserialize<DownloadFileRequest>();
						break;

					case CreateDirectoryRequest.Identifier:
						Deserialize<CreateDirectoryRequest>();
						break;

					case UploadFileRequest.Identifier:
						Deserialize<UploadFileRequest>();
						break;

					default:
						protocolResponse = new ErrorResponse
						{
							Code = 400,
							Message = "Unsupported request."
						};
						break;
				}

				void Deserialize<TRequest>()
					where TRequest : IProtocolRequest<IProtocolResponse>
				{
					try
					{
						request = ProtocolBodySerializer.Deserialize<TRequest>(requestObject.Body);
						protocolResponse = executor.Execute(request);
					}
					catch (AuthorizationException)
					{
						protocolResponse = new ErrorResponse
						{
							Code = 403,
							Message = "Unauthorized."
						};
					}
					catch (CustomException e)
					{
						protocolResponse = new ErrorResponse
						{
							Code = 400,
							Message = e.Message
						};
					}
					catch (Exception)
					{
						protocolResponse = new ErrorResponse
						{
							Code = 500,
							Message = "Internal server error."
						};
					}
				}

				MessageHeadersDictionary responseHeaders = new MessageHeadersDictionary();

				string identifier = protocolResponse.GetIdentifier();
				string headers = ProtocolMessageHeaderSerializer.Serialize(responseHeaders);
				string serializedBody = ProtocolBodySerializer.Serialize(protocolResponse);
				string serializedResponse = ProtocolSerializer.Serialize(identifier, headers, serializedBody);

				byte[] bytes = encoder.Encode(serializedResponse);

				connection.Send(bytes);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static void OnErrorOccured(ErrorContext ctx)
		{
			string message = ctx?.Exception?.Message ?? "Unidentified error occured.";

			Console.WriteLine(message);
		}
	}
}