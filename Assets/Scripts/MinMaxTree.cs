using UnityEngine;
using System.Linq;

public class MinMaxTree 
{
	static MinMaxTree _instance;
	public static MinMaxTree Instance 
	{ 
		get 
		{ 
			if(_instance == null)
				_instance = new MinMaxTree();
			return _instance;
		} 
	}

	MinMaxNode<int> root;

	MinMaxTree()
	{
		// TODO:
	}

	public void PrepareTree()
	{}

	// TODO: cache some predicates
	public bool TryApplyMove(ref MinMaxNode<int> node, PlayersMove move)
	{
		MinMaxNode<int> nextNode = node.Children.FirstOrDefault(n => n.Data == move.Hash);
		if(nextNode != null)
		{
			node = nextNode;
			return true;
		}

		return false;
	}

	public PlayersMove? ApplyBestMove(ref MinMaxNode<int> node, bool maximize)
	{
		if(node.Children == null || node.Children.Length == 0)
			return null;

		MinMaxNode<int> nextNode = maximize ? node.GetMaxChild() : node.GetMinChild();

		node = nextNode;
		return new PlayersMove(nextNode.Data);
	}
}
