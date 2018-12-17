using LazyTransportProtocol.Core.Application.Protocol.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LazyTransportProtocol.Core.Application.Protocol.Services
{
	public class AvailableRequestsContainer
	{
		private readonly Dictionary<string, Type> _availableRequests = new Dictionary<string, Type>();

		public void RegisterByType<T>(Func<T, string> getIdentifier)
		{
			Type[] matchingTypes = GetType().Assembly.GetTypes()
				.Where(x => typeof(T).IsAssignableFrom(x)).ToArray();

			foreach (Type matchingType in matchingTypes)
			{
				// Get public parameterless constructor
				ConstructorInfo parameterlessConstructor = matchingType.GetConstructor(new Type[0]);

				if (parameterlessConstructor == null)
				{
					continue;
				}

				T request = (T)Activator.CreateInstance(matchingType);
				string identifier = getIdentifier(request);

				if (_availableRequests.ContainsKey(identifier))
				{
					throw new Exception("Another request with same identifier was already registered." +
						"Duplicate identifier: " + identifier);
				}

				_availableRequests.Add(identifier, matchingType);
			}
		}

		public Type GetRequestType(string identifier)
		{
			if (_availableRequests.TryGetValue(identifier, out Type value))
			{
				return value;
			}

			return null;
		}

		public Dictionary<string, Type> GetAll()
		{
			// Create copy of internal dictionary
			return _availableRequests.ToDictionary(x => x.Key, x => x.Value);
		}
	}
}