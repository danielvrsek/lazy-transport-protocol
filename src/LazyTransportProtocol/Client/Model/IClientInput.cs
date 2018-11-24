using System;
using System.Collections.Generic;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
    public interface IClientInput
    {
		void Execute(string[] parameters);
    }
}
