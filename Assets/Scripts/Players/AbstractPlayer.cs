using UnityEngine;

public interface IUserControlledPlayer
{}

public abstract class AbstractPlayer 
{
	protected GameBoardController board;

	public event System.Action<PlayersMove> OnMoveCompleted;
	public TileMark Type { get; private set; }

	public AbstractPlayer(GameBoardController board, TileMark defaultType)
	{
		this.board = board;
		this.PrepareForGame(defaultType);
	}

	public virtual void PrepareForGame(TileMark playerType)
	{
		if(playerType == TileMark.Empty)
		{
			Debug.LogError("Tried to set Player to Empty type");
			this.Type = TileMark.Cross;
			return;
		}

		this.Type = playerType;
	}
		
	public abstract void StartMove(PlayersMove? previousMove);

	protected void FireOnMoveCompleted(PlayersMove move)
	{
		if(this.OnMoveCompleted != null)
			this.OnMoveCompleted(move);
	}
}
