using UnityEngine;

public interface IUserControlledPlayer
{}

public abstract class AbstractPlayer 
{
	protected GameBoard board;

	public event System.Action<PlayersMove> OnMoveCompleted;
	public TileState Type { get; private set; }

	public AbstractPlayer(GameBoard board, TileState defaultType)
	{
		this.board = board;
		this.PrepareForGame(defaultType);
	}

	public virtual void PrepareForGame(TileState playerType)
	{
		if(playerType == TileState.Empty)
		{
			Debug.LogError("Tried to set Player to Empty type");
			this.Type = TileState.Cross;
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
