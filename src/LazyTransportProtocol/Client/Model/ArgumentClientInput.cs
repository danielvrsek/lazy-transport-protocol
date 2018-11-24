using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Validators;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public class ArgumentClientInput<TModel> : IClientInput
		where TModel : class, new()
	{
		private Action<TModel> _actionToExecute;

		private Dictionary<string, Container<TModel>> _argumentsDictionary = new Dictionary<string, Container<TModel>>();
		private Dictionary<int, Container<TModel>> _indexedDictionary = new Dictionary<int, Container<TModel>>();

		public ArgumentClientInput(Action<TModel> actionToExecute)
		{
			_actionToExecute = actionToExecute;
		}

		public void Execute(string[] parameters)
		{
			TModel model = new TModel();

			for (int i = 0; i < parameters.Length; i++)
			{
				if (IsArgument(parameters[i]))
				{
					string argument = parameters[i].Replace("-", "");
					string parameter = ++i < parameters.Length && !IsArgument(parameters[i])?
						parameters[i] :
						throw new CommandException("Parameter expected.");

					IValidator<string> validator = _argumentsDictionary.GetValueOrDefault(argument)?.Validator;
					if (validator?.Validate(parameter) == false)
					{
						throw new ValidationException();
					}

					if (_argumentsDictionary.ContainsKey(argument))
					{
						_argumentsDictionary[argument].Action(parameter, model);
					}

					_argumentsDictionary.Remove(argument);
				}
				else
				{
					IValidator<string> validator = _indexedDictionary.GetValueOrDefault(i)?.Validator;
					if (validator?.Validate(parameters[i]) == false)
					{
						throw new ValidationException();
					}

					if (_indexedDictionary.ContainsKey(i))
					{
						_indexedDictionary[i].Action(parameters[i], model);
					}

					_indexedDictionary.Remove(i);
				}
			}

			if (_argumentsDictionary.Any() && _indexedDictionary.Any())
			{
				throw new CommandException("Insufficient arguments.");
			}

			_actionToExecute(model);
		}

		private bool IsArgument(string value)
		{
			return value.StartsWith('-');
		}

		public ArgumentClientInput<TModel> RegisterArgument(string argument, Action<string, TModel> assignAction, string defaultValue = null)
		{
			argument = argument.Replace("-", "");

			Container<TModel> container = new Container<TModel>();

			container.Action = assignAction;
			container.IsSecureString = isSecureString;

			_argumentsDictionary[argument] = container;

			return this;
		}

		public ArgumentClientInput<TModel> RegisterParameter(int index, Action<string, TModel> assignAction, IValidator<string> validator, bool isRequired, bool promtIfEmpty, bool isSecureString)
		{
			Container<TModel> container = new Container<TModel>
			{
				Validator = validator,
				Action = assignAction,
				IsRequired = isRequired,
				PromtIfEmpty = promtIfEmpty,
				IsSecureString = isSecureString
			};

			_indexedDictionary[index] = container;

			return this;
		}

		private class Container<T>
		{
			public Action<string, T> Action { get; set; }

			public IValidator<string> Validator { get; set; }

			public bool IsRequired { get; set; }

			public bool PromtIfEmpty { get; set; }

			public bool IsSecureString { get; set; }
		}
	}
}
