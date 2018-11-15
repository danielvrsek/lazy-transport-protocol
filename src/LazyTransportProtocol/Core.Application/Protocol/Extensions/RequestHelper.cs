using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Requests;
using LazyTransportProtocol.Core.Application.Protocol.Abstractions.Responses;
using LazyTransportProtocol.Core.Application.Protocol.Infrastucture;
using LazyTransportProtocol.Core.Application.Protocol.Metadata;
using LazyTransportProtocol.Core.Application.Protocol.Services;
using LazyTransportProtocol.Core.Application.Protocol.ValueTypes;
using LazyTransportProtocol.Core.Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Extensions
{
	public static class RequestHelper
	{
		public static void Deserialize<TResponse>(this IProtocolRequest<TResponse> request, MediumDeserializedRequestObject requestObject, ProtocolVersion protocolVersion)
			where TResponse : class, IProtocolResponse, new()
		{
			if (requestObject == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(requestObject));
			}

			string complexPropertyIndicator = ":";

			PropertyInfo[] properties = request.GetType().GetProperties();

			if (request.GetIdentifier(protocolVersion) != requestObject.ControlCommand)
			{
				throw new RequestMismatchException();
			}

			foreach (string parameterName in requestObject.Parameters.AllKeys)
			{
				PropertyInfo matchingProperty = properties.SingleOrDefault(x => x.Name.ToLower() == parameterName.ToLower());

				if (matchingProperty == null)
				{
					throw new InvalidRequestException();
				}

				string parameterValue = requestObject.Parameters[parameterName];

				if (parameterValue[0] == ':')
				{
					// Complex parameter
					matchingProperty.DeserializeComplexProperty(request, complexPropertyIndicator, parameterValue);
				}
				else
				{
					matchingProperty.DeserializeScalarProperty(request, parameterValue);
				}
			}
		}

		private static void DeserializeScalarProperty(this PropertyInfo property, object obj, string value)
		{
			switch (property.PropertyType)
			{
				case Type t when t == typeof(string):
					property.SetValue(obj, value);
					break;

				case Type t when t == typeof(int):
					if (!Int32.TryParse(value, out int intValue))
					{
						throw new InvalidRequestException();
					}
					property.SetValue(obj, intValue);
					break;

				default:
					throw new NotSupportedException("Only int and string are supported.");
			}
		}

		private static void DeserializeComplexProperty(this PropertyInfo complexProperty, object obj, string complexPropertyIndicator)
		{
			object complexPropertyInstance = Activator.CreateInstance(complexProperty.PropertyType);

			PropertyInfo[] properties = complexProperty.PropertyType.GetProperties();

			foreach (PropertyInfo property in properties)
			{
			}
		}

		public static string Serialize<TResponse>(this IProtocolRequest<TResponse> request, AgreedHeadersDictionary headers, ProtocolVersion protocolVersion)
			where TResponse : class, IProtocolResponse, new()
		{
			if (headers == null)
			{
				throw new ArgumentException("Argument cannot be null.", nameof(headers));
			}

			PropertyInfo[] properties = request.GetType().GetProperties();

			StringBuilder sb = new StringBuilder();

			string parameterSeparator = ",";
			string complexPropertyIndicator = ":";

			foreach (PropertyInfo property in properties)
			{
				try
				{
					string serialized = property.SerializeProperty(request, complexPropertyIndicator, parameterSeparator);

					sb.Append(serialized + parameterSeparator);
				}
				catch
				{
					// Not supported type. Skip
				}

				continue;
			}

			return sb.ToString();
		}

		private static string SerializeProperty(this PropertyInfo property, object obj, string complexPropertyIndicator, string parameterSeparator)
		{
			if (typeof(IComplexParameter).IsAssignableFrom(property.PropertyType))
			{
				return property.SerializeComplexProperty(obj, complexPropertyIndicator, parameterSeparator);
			}
			else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
			{
			}
			else
			{
				return property.SerializeScalarProperty(obj);
			}
		}

		private static string SerializeComplexProperty(this PropertyInfo complexProperty, object obj, string complexPropertyIndicator, string parameterSeparator)
		{
			if (!typeof(IComplexParameter).IsAssignableFrom(complexProperty.PropertyType))
			{
				// The type of the property does not inherit from IComplexParameter
				throw new ArgumentException($"Only properties that inherit from {nameof(IComplexParameter)} can be serialized.", nameof(complexProperty));
			}

			object propertyValue = complexProperty.GetValue(obj);

			PropertyInfo[] properties = complexProperty.PropertyType.GetProperties();
			StringBuilder sb = new StringBuilder();

			sb.Append($"{complexProperty.Name.ToLower()}={complexPropertyIndicator}");

			foreach (PropertyInfo property in properties)
			{
				sb.Append(property.SerializeProperty(propertyValue, complexPropertyIndicator, parameterSeparator) + parameterSeparator);
			}

			return sb.ToString();
		}

		private static string SerializeEnumerableProperty(this PropertyInfo enumerableProperty, object obj, string complexPropertyIndicator, string parameterSeparator)
		{
			if (!typeof(IEnumerable).IsAssignableFrom(enumerableProperty.PropertyType))
			{
				// The type of the property does not inherit from IComplexParameter
				throw new ArgumentException($"Only properties that inherit from {nameof(IEnumerable)} can be serialized.", nameof(enumerableProperty));
			}

			IEnumerable value = (IEnumerable)enumerableProperty.GetValue(obj);

			foreach (object o in value)
			{
			}
		}

		private static string SerializeScalarProperty(this PropertyInfo property, object obj)
		{
			if (property.PropertyType != typeof(string) || property.PropertyType != typeof(int))
			{
				throw new NotSupportedException("Only int and string are supported.");
			}

			object value = property.GetValue(obj);

			return $"{property.Name.ToLower()}={value.ToString()}";
		}
}