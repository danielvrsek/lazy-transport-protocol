using LazyTransportProtocol.Core.Application.Infrastructure;
using LazyTransportProtocol.Core.Application.Protocol;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Model;
using LazyTransportProtocol.Core.Application.Protocol.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Application.Transport;
using LazyTransportProtocol.Core.Domain.Abstractions;
using LazyTransportProtocol.Core.Domain.Abstractions.Common;
using LazyTransportProtocol.Core.Domain.Exceptions;
using LazyTransportProtocol.Core.Domain.Exceptions.Authorization;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
				IDecoder decoder = new ProtocolDecoder();
				IEncoder encoder = new ProtocolEncoder();

				string requestString = decoder.Decode(data);

				MediumDeserializedObject requestObject = null;
				try
				{
					requestObject = ProtocolDeserializer.Deserialize(requestString);
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
						request = ProtocolBodyDeserializer.Deserialize<CreateUserRequest>(requestObject.Body);
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

		private static void OnErrorOccured(IClientConnection connection, Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
}