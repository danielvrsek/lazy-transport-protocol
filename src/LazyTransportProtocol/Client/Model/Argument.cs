using LazyTransportProtocol.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyTransportProtocol.Client.Model
{
	public class Argument<TModel> : IArgument<TModel>
	{
		private readonly string _argumentName = null;
		private readonly int _index = -1;

		private Action<string, TModel> _assignAction;

		private Action<TModel> _onEmpty;

		public Argument(string argumentName, Action<string, TModel> assignAction)
		{
			_argumentName = argumentName;
			_assignAction = assignAction;
		}

		public Argument(int index, Action<string, TModel> assignAction)
		{
			_index = index;
			_assignAction = assignAction;
		}

		public bool Process(string[] parameters, TModel model)
		{
			if (parameters.Length == 0)
			{
				return false;
			}

			bool isEmpty = true;

			if (_index > 0)
			{
				if (parameters.Length > _index)
				{
					isEmpty = false;
					_assignAction(parameters[_index], model);
				}
			}
			else if (_argumentName != null)
			{
				int argumentIndex = -1;

				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i] == _argumentName)
					{
						argumentIndex = i;
						break;
					}
				}

				if (argumentIndex > 0)
				{
					if (parameters.Length > argumentIndex + 1 || IsArgument(parameters[argumentIndex + 1]))
					{
						throw new CommandException("Parameter expected.");
					}

					isEmpty = false;
					_assignAction(parameters[argumentIndex + 1], model);
				}
			}

			if (isEmpty && _onEmpty != null)
			{
				_onEmpty(model);

				return true;
			}

			return false;
		}

		public void PromtIfEmpty(string title, bool secure = false)
		{
			_onEmpty = model =>
			{
				Console.Write(title + ": ");
				string value = secure ? ReadSecureString() : Console.ReadLine();
				_assignAction(value, model);
			};
		}

		public void SetDefaultValue(string defaultValue)
		{
			_onEmpty = model =>
			{
				_assignAction(defaultValue, model);
			};
		}

		private bool IsArgument(string value)
		{
			return value.StartsWith('-');
		}

		private string ReadSecureString()
		{
			StringBuilder sb = new StringBuilder();

			ConsoleKeyInfo key;

			while (true)
			{
				key = Console.ReadKey(true);

				if (key.Key == ConsoleKey.Enter)
				{
					Console.WriteLine();
					break;
				}
				else if (key.Key == ConsoleKey.Backspace)
				{
					if (sb.Length > 0)
					{
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
						Console.Write(' ');
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

						if (sb.Length > 1)
						{
							sb.Remove(sb.Length - 2, 1);
						}
						else
						{
							sb.Remove(0, 1);
						}
					}
				}
				else
				{
					Console.Write('*');
					sb.Append(key.KeyChar);
				}
			}

			return sb.ToString();
		}
	}
}
