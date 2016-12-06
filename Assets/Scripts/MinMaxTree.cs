using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// TODO: implement more complex scoring system
public class MinMaxTree 
{
	// TODO: do not use duplicate for struct that is defined in GameBoard
	public class BoardState
	{
		int[] tiles = new int[9];
		int[][] checkIdxs = new int[8][] {
			new int[3] { 0, 1, 2 },
			new int[3] { 3, 4, 5 },
			new int[3] { 6, 7, 8 },

			new int[3] { 0, 3, 6 },
			new int[3] { 1, 4, 7 },
			new int[3] { 2, 5, 8 },

			new int[3] { 0, 4, 8 },
			new int[3] { 2, 4, 6 }
		};
		public int FilledCount { get; private set; }

		public int this[int idx] { get { return this.tiles[idx]; } }

		public void SetTile(int idx, int type)
		{
			// TODO: check filled count
			if(type == 0)
			{
				if(this.tiles[idx] > 0)
					--this.FilledCount;
			}
			else
			{
				if(this.tiles[idx] == 0)
					++this.FilledCount;
			}

			this.tiles[idx] = type;
		}

		// -1 not over yet, 0 draw, 1 cross, 2 circle
		public int GetWinner()
		{
			int[] t = this.tiles;

			for(int i = 0; i < 8; ++i)
			{
				int[] idxs = this.checkIdxs[i];

				int val0 = t[idxs[0]];
				int val1 = t[idxs[1]];
				int val2 = t[idxs[2]];

				if(val0 != 0 && val0 == val1 && val1 == val2)
					return val0;
			}

			return this.FilledCount < 9 ? -1 : 0;
		}
	}

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

	int[] nodesCountByDepth = new int[9];
	string GetNodesCountByDepth()
	{
		string result = "\n";
		for(int i = 0; i < this.nodesCountByDepth.Length; ++i)
			result += i + ": " + this.nodesCountByDepth[i] + "\n";
		return result;
	}

	public MinMaxNode<int> Root { get; private set; }

	MinMaxTree()
	{
		Root = new MinMaxNode<int>(0, 0);
		System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
		this.AddChildren(Root, new BoardState(), 0);
		timer.Stop();
#if DEBUG
		Debug.LogFormat("MinMaxTree nodes count: {0}. Construction time: {1}. Check tree: {2}", this.GetNodesCountByDepth(), timer.Elapsed.TotalSeconds, this.CheckTree(Root, 9));
#endif
	}

#if DEBUG
	bool CheckTree(MinMaxNode<int> node, int maxChildren)
	{
		if(node.Children == null)
			return true;

		if(node.Children.Length > maxChildren)
			return false;

		for(int i = 0; i < node.Children.Length; ++i)
		{
			if(!CheckTree(node.Children[i], maxChildren - 1))
				return false;
		}

		return true;
	}
#endif
		
	// TODO: can reduce node count by not adding nodes corresponding to turn filling last tile
	int AddChildren(MinMaxNode<int> node, BoardState state, int depth)
	{
		MinMaxNode<int>[] children = new MinMaxNode<int>[9 - depth];
		int playerType = depth % 2;
		int childIdx = -1;

		for(int i = 0; i < 9; ++i)
		{
			if(state[i] == 0)
			{
				PlayersMove move = new PlayersMove(i / 3, i % 3);
				MinMaxNode<int> child = new MinMaxNode<int>(move.Hash, 0);

				state.SetTile(i, playerType + 1);
				int winner = state.GetWinner();

				if(winner != -1)
					child.Score = winner == 1 ? int.MaxValue : (winner == 2 ? int.MinValue : 0);
				else
					child.Score = this.AddChildren(child, state, depth + 1);
						
				// Restore board state as it was before this move
				state.SetTile(i, 0);

				// Add child to current node
				children[++childIdx] = child;
				++this.nodesCountByDepth[depth];
			}
		}
			
		System.Array.Sort<MinMaxNode<int>>(children);
		node.Children = children;
		return (playerType == 0 ? node.MaxChild : node.MinChild).Score;
	}

	public void PrepareTree()
	{}

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

		MinMaxNode<int> nextNode = maximize ? node.MaxChild : node.MinChild;

		node = nextNode;
		return new PlayersMove(nextNode.Data);
	}

	public PlayersMove? ApplyRandomMove(ref MinMaxNode<int> node)
	{
		if(node.Children == null || node.Children.Length == 0)
			return null;

		node = node.RandomChild;
		return new PlayersMove(node.Data);
	}
}
