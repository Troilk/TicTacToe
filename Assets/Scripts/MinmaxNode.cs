using UnityEngine;

public class MinMaxNode<T> : System.IComparable<MinMaxNode<T>>
{
	public T Data;
	public int Score;
	public MinMaxNode<T>[] Children;

	public MinMaxNode<T> MinChild { get { return this.Children[0]; } }
	public MinMaxNode<T> MaxChild { get { return this.Children[this.Children.Length - 1]; } }
	public MinMaxNode<T> RandomChild { get { return this.Children[Random.Range(0, this.Children.Length)]; } }

	public MinMaxNode(T data, int score)
	{
		this.Data = data;
		this.Score = score;
	}

	public int CompareTo(MinMaxNode<T> other)
	{
		return this.Score.CompareTo(other.Score);
	}
}
