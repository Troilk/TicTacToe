using UnityEngine;
using System.Collections.Generic;

public class MinMaxNode<T> 
{
	public T Data;
	public int Score;
	public MinMaxNode<T>[] Children;

	public MinMaxNode(T data, int score)
	{
		this.Data = data;
		this.Score = score;
	}

	public MinMaxNode<T> GetMaxChild()
	{
		int bestScore = int.MinValue;
		int bestNodeIdx = -1;

		for(int i = 0; i < this.Children.Length; ++i)
		{
			if(this.Children[i].Score > bestScore)
			{
				bestScore = this.Children[i].Score;
				bestNodeIdx = i;
			}
		}

		return bestNodeIdx == -1 ? null : this.Children[bestNodeIdx];
	}

	public MinMaxNode<T> GetMinChild()
	{
		int bestScore = int.MaxValue;
		int bestNodeIdx = -1;

		for(int i = 0; i < this.Children.Length; ++i)
		{
			if(this.Children[i].Score < bestScore)
			{
				bestScore = this.Children[i].Score;
				bestNodeIdx = i;
			}
		}

		return bestNodeIdx == -1 ? null : this.Children[bestNodeIdx];
	}
}
