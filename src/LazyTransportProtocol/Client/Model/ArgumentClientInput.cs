using LazyTransportProtocol.Client.Exceptions;
using LazyTransportProtocol.Client.Validators;
using LazyTransportProtocol.Core.Domain.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using static LazyTransportProtocol.Client.Model.ArgumentClientInput;

namespace LazyTransportProtocol.Client.Model
{
	public static class ArgumentClientInput
	{
		public enum Behavior
		{
			ThrowException,
			Retry
		}
	}

	public class ArgumentClientInput<TModel> : IClientInput
		where TModel : class, new()
	{
		private Action<TModel> _actionToExecute;
		private Behavior _behavior;

		private List<IArgument<TModel>> _arguments = new List<IArgument<TModel>>();
		public ArgumentClientInput(Action<TModel> actionToExecute, Behavior behavior = Behavior.ThrowException)
		{
			_actionToExecute = actionToExecute;
		}

		public ArgumentClientInput<TModel> RegisterArgument(IArgument<TModel> argument)
		{
			_arguments.Add(argument);

			return this;
		}

		public void Execute(string[] parameters)
		{
			TModel model = new TModel();

			foreach (var argument in _arguments)
			{
				if (!argument.Process(parameters, model))
				{
					if (_behavior == Behavior.ThrowException)
					{
						throw new CommandException("Invalid arguments.");
					}
					else if (_behavior == Behavior.Retry)
					{
						while (!argument.Process(parameters, model)) ;
					}
				}
			}

			_actionToExecute(model);
		}
	}
}
