using UnityEngine;

public class MinMaxAIPlayer : AbstractPlayer 
{
	float errorProbability;
	MinMaxNode<int> currentNode;

	public MinMaxAIPlayer(GameBoardController board, TileMark playerType, float errorProbability) 
		: base(board, playerType)
	{
		this.errorProbability = errorProbability;
		MinMaxTree.Instance.PrepareTree();
	}

	public override void PrepareForGame(TileMark playerType)
	{
		base.PrepareForGame(playerType);
		this.currentNode = MinMaxTree.Instance.Root;
	}

	public override void StartMove(PlayersMove? previousMove)
	{
		// Take into account move of opposing player
		if(previousMove != null)
		{
			if(!MinMaxTree.Instance.TryApplyMove(ref this.currentNode, previousMove.Value))
				throw new UnityException("Cannot apply enemy move");
		}

		PlayersMove? move = Random.value < this.errorProbability ?
			MinMaxTree.Instance.ApplyRandomMove(ref this.currentNode) :
			MinMaxTree.Instance.ApplyBestMove(ref this.currentNode, this.Type == TileMark.Cross);

		if(move == null)
			throw new UnityException("MinMaxAIPlayer has no option to make a move...Game should have probably been over already");
		this.FireOnMoveCompleted(move.Value);
	}
}
