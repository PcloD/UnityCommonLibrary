using System;

namespace UnityCommonLibrary.Events
{
	public interface IEventListener<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		void OnEvent(T evt, EventData data);
	}
}
