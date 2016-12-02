using UnityEngine;
using System.Collections.Generic;

public class ReadonlyList<T>
{
	List<T> items;

	public T this[int key] { get { return this.items[key]; } }
	public int Count { get { return this.items.Count; } }

	public ReadonlyList(T[] items)
	{
		this.items = new List<T>(items);
	}

	public ReadonlyList()
	{
		this.items = new List<T>();
	}

	public void Add(T item)
	{
		this.items.Add(item);
	}

	public T GetRandom()
	{
		if(this.items.Count == 0)
			return default(T);
		return this.items[Random.Range(0, this.items.Count)];
	}
}
