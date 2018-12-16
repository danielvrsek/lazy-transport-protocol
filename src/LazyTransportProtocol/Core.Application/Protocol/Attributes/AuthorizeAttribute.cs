using System;

namespace LazyTransportProtocol.Core.Application.Protocol.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public sealed class AuthorizeAttribute : Attribute
	{
		public AuthorizeAttribute()
		{
		}
	}
}