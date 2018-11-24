using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public static class ArgumentCondition
	{
		public static ArgumentCondition<TModel> If<TModel>(IArgument<TModel> condition, IArgument<TModel> first, IArgument<TModel> second)
		{
			return new ArgumentCondition<TModel>((parameters, model) =>
			{
				if (condition.Process(parameters, model))
				{
					return first.Process(parameters, model);
				}

				return second.Process(parameters, model);
			});
		}

		public static ArgumentCondition<TModel> Or<TModel>(IArgument<TModel> first, IArgument<TModel> second)
		{
			return new ArgumentCondition<TModel>((parameters, model) =>
			{
				if (!first.Process(parameters, model) 
					&& !second.Process(parameters, model))
				{
					return false;
				}

				return true;
			});
		}
	}

	public class ArgumentCondition<TModel> : IArgument<TModel>
	{
		private Func<string[], TModel, bool> _eval;

		internal ArgumentCondition(Func<string[], TModel, bool> eval)
		{
			_eval = eval;
		}

		public bool Process(string[] parameters, TModel model)
		{
			return _eval(parameters, model);
		}
	}
}
