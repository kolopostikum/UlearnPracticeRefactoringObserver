using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
	public delegate	void HandleEventDelegate(object eventData);
	public class StackOperationsLogger
	{
		public StringBuilder Log = new StringBuilder();
		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(HandleEvent);
		}

		public string GetLog()
		{
			return Log.ToString();
		}
	}

	public class ObservableStack<T>
	{
		public event HandleEventDelegate observers;

		public void Add(HandleEventDelegate observer)
		{
			observers+=observer;
		}

		public void Notify(object eventData)
		{
			if (observers!=null)
				observers(eventData);
		}

		public void Remove(HandleEventDelegate observer)
		{
			observers-=observer;
		}

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}
