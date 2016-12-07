using UnityEngine;

public class UserPlayer : AbstractPlayer, IUserControlledPlayer
{
	public UserPlayer(GameBoardController board, TileMark playerType) 
		: base(board, playerType)
	{}

	public override void StartMove(PlayersMove? previousMove)
	{
		// Enable player input
		var input = this.board.InputController;
		input.Interactable = true;
		input.OnTileClicked += this.OnTileClicked;
	}

	void OnTileClicked(IGameBoardTileView tile)
	{
		// ignore clicks on non-empty tiles
		if(this.board[tile.Row, tile.Col] != TileMark.Empty)
			return;

		var input = this.board.InputController;
		input.Interactable = false;
		input.OnTileClicked -= this.OnTileClicked;

		this.FireOnMoveCompleted(new PlayersMove(tile.Row, tile.Col));
	}
}
