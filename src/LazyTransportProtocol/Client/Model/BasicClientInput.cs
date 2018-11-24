using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public class BasicClientInput : IClientInput
	{
		private Dictionary<int, IValidator<string>> _validatorDictionary = new Dictionary<int, IValidator<string>>();
		public void Execute(string[] parameters)
		{
			
		}

		public void RegisterValidator(int parameterIndex, IValidator<string> validator)
		{
			_validatorDictionary[parameterIndex] = validator;
		}
	}
}
