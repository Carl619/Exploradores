using System;
using System.Collections.Generic;




namespace PriorityQueue
{
	
	
	public class PQ<T, P> : SortedDictionary<P, List<T>>
		where T : class
		where P : IComparable
	{
		// constructor
		public PQ()
			: base()
		{
		}


		// functions
		public void push(T element, P priority)
		{
			List<T> list;
			if(TryGetValue(priority, out list) == true)
			{
				list.Add(element);
				return;
			}
			list = new List<T>();
			list.Add(element);
			Add(priority, list);
		}


		public T pop()
		{
			KeyValuePair<P, List<T>> first = default(KeyValuePair<P, List<T>>);
			bool found = false;
			foreach(KeyValuePair<P, List<T>> elem in this)
			{
				if(first.Key.CompareTo(elem.Key) > 0 || found == false)
				{
					first = elem;
					found = true;
				}
			}

			if(found == true)
			{
				if(first.Value.Count == 0)
				{
					Remove(first.Key);
					return null;
				}
				T element = first.Value[0];
				first.Value.RemoveAt(0);
				if(first.Value.Count == 0)
					Remove(first.Key);
				return element;
			}
			return null;
		}
	}
}




