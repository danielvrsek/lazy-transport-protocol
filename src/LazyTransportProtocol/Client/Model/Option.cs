using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public static class Option
	{
		public static Option<TModel> Create<TModel>(Action<bool, TModel> assignAction, params string[] argumentNames)
		{
			return new Option<TModel>(assignAction, argumentNames);
		}
	}

	public class Option<TModel> : IArgument<TModel>
	{
		private readonly string[] _argumentNames = null;
		private readonly Action<bool, TModel> _assignAction;

		public Option(Action<bool, TModel> assignAction, string[] argumentNames)
		{
			_assignAction = assignAction;
			_argumentNames = argumentNames;
		}

		public bool Process(string[] parameters, TModel model)
		{
			if (parameters.Any(x => _argumentNames.Contains(x)))
			{
				_assignAction(true, model);
			}
			else
			{
				_assignAction(false, model);
			}

			return true;
		}
	}
}
