using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Core.Domain.Exceptions
{
	public class Contract
	{
		public static void Requires<TException>(bool condition)
			where TException : Exception, new()
		{
			if (!condition)
			{
				throw new TException();
			}
		}

		public static void Requires(bool condition)
		{
			if (!condition)
			{
				throw new ContractException();
			}
		}
	}
}