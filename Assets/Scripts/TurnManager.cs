using UnityEngine;
using System.Linq;

public class TurnManager 
{
	private struct GameInfo
	{
		public int WinnerIdx;
		public int StartingPlayerIdx;

		public GameInfo(int winnerIdx, int startingPlayerIdx)
		{
			this.WinnerIdx = winnerIdx;
			this.StartingPlayerIdx = startingPlayerIdx;
		}
	}

	GameBoardController gameBoard;
	AbstractPlayer[] players;

	int currentPlayerIdx = 0;
	GameInfo previousGameInfo = new GameInfo(0, 0);

	AbstractPlayer currentPlayer { get { return this.players[this.currentPlayerIdx]; } }

	public event System.Action<AbstractPlayer> OnGameOver;

	public TurnManager(GameBoardController board, params AbstractPlayer[] players)
	{
		this.gameBoard = board;
		this.players = players;
		this.dOnMoveCompleted = this.OnMoveCompleted;
	}

	public void Start()
	{
		// Decide who goes first 
		if(this.previousGameInfo.WinnerIdx != -1)
			this.currentPlayerIdx = this.previousGameInfo.WinnerIdx;
		else
			this.currentPlayerIdx = (this.previousGameInfo.StartingPlayerIdx + 1) % this.players.Length;

		// Save starter player idx for next game
		this.previousGameInfo.StartingPlayerIdx = this.currentPlayerIdx;

		// Prepare players for game
		for(int i = 0; i < this.players.Length; ++i)
			this.players[i].PrepareForGame(i == this.currentPlayerIdx ? TileMark.Cross : TileMark.Circle);

		// Start first turn
		this.StartNextTurn(null);
	}

	void StartNextTurn(PlayersMove? previousMove)
	{
		// Update current player index, if it's not the first turn in game
		if(previousMove != null)
			this.currentPlayerIdx = (this.currentPlayerIdx + 1) % this.players.Length;

#if DEBUG
		Debug.LogFormat("<color=blue>Starting player {0} move</color>", this.currentPlayerIdx);
#endif

		// Start player's move
		this.currentPlayer.OnMoveCompleted += this.dOnMoveCompleted;
		EventRelay.FireOnTurnStarted(this.currentPlayer);
		this.currentPlayer.StartMove(previousMove);
	}

	System.Action<PlayersMove> dOnMoveCompleted;
	void OnMoveCompleted(PlayersMove move)
	{
		if(this.ValidateMove(move))
		{
			// Update game board
			this.currentPlayer.OnMoveCompleted -= this.dOnMoveCompleted;
			this.gameBoard.SetTileState(move.Row, move.Col, this.currentPlayer.Type);

			// Check if game is over
			TileMark? winningType;
			if(this.gameBoard.CheckGameOverCondition(out winningType))
			{
				// Winner could be only the player, who was making last turn
				this.previousGameInfo.WinnerIdx = winningType == null ? -1 : this.currentPlayerIdx;

				if(this.OnGameOver != null)
					this.OnGameOver(winningType == null ? null : this.players[this.currentPlayerIdx]);
				return;
			}

			// If not game over - start next turn
			this.StartNextTurn(move);
		}
	}

	bool ValidateMove(PlayersMove move)
	{
		return this.gameBoard[move.Row, move.Col] == TileMark.Empty;
	}
}
