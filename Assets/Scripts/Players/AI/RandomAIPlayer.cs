using UnityEngine;

public class RandomAIPlayer : AbstractPlayer
{
	public RandomAIPlayer(GameBoardController board, TileMark playerType) 
		: base(board, playerType)
	{}

	public override void StartMove(PlayersMove? previousMove)
	{
		int row, col;
		this.GetRandomCoords(out row, out col);

		while(this.board[row, col] != TileMark.Empty)
			this.GetRandomCoords(out row, out col);

		this.FireOnMoveCompleted(new PlayersMove(row, col));
	}

	void GetRandomCoords(out int row, out int col)
	{
		row = Random.Range(0, this.board.Rows);
		col = Random.Range(0, this.board.Cols);
	}
}
