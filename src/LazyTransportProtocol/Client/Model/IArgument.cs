using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public interface IArgument<TModel>
	{
		bool Process(string[] parameters, TModel model);
	}
}
