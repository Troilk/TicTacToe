using UnityEngine;

public class UserPlayer : AbstractPlayer, IUserControlledPlayer
{
	public UserPlayer(GameBoard board, TileState playerType) 
		: base(board, playerType)
	{
		// TODO:
	}

	public override void StartMove(PlayersMove? previousMove)
	{
		var input = this.board.InputController;

		// Enable player input
		// TODO: who should really be responsible for enabling/disabling view interactivity ?
		input.Interactable = true;
		input.OnTileClicked += this.OnTileClicked;
	}

	void OnTileClicked(GameBoard.IGameBoardTileView tile)
	{
		// ignore clicks on non-empty tiles
		if(this.board[tile.Row, tile.Col] != TileState.Empty)
			return;

		var input = this.board.InputController;
		input.Interactable = false;
		input.OnTileClicked -= this.OnTileClicked;

		this.FireOnMoveCompleted(new PlayersMove(tile.Row, tile.Col));
	}
}
