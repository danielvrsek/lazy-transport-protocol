namespace LazyTransportProtocol.Client.Model
{
	public interface IArgument<TModel>
	{
		bool Process(string[] parameters, TModel model);
	}
}