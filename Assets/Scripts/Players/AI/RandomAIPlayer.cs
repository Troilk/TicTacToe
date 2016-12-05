using UnityEngine;

public class RandomAIPlayer : AbstractPlayer
{
	public RandomAIPlayer(GameBoard board, TileState playerType) 
		: base(board, playerType)
	{
		// TODO:
	}

	public override void StartMove(PlayersMove? previousMove)
	{
		int row, col;
		this.GetRandomCoords(out row, out col);

		while(this.board[row, col] != TileState.Empty)
			this.GetRandomCoords(out row, out col);

		this.FireOnMoveCompleted(new PlayersMove(row, col));
	}

	void GetRandomCoords(out int row, out int col)
	{
		row = Random.Range(0, this.board.Rows);
		col = Random.Range(0, this.board.Cols);
	}
}
