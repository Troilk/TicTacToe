using UnityEngine;

public enum TileMark
{
	Empty = 0,
	Cross,
	Circle
}

#region Interfaces

public interface IGameBoardView
{
	void Init(int rows, int cols);
	void RefreshTile(int row, int col, TileMark mark);
	void Refresh(IGameBoardModel model);
	IGameBoardInputController InputController { get; }
}

public interface IGameBoardModel
{
	int Rows { get; }
	int Cols { get; }
	TileMark this[int row, int col] { get; }
	bool TrySetPlayerMark(int row, int col, TileMark mark);
	bool CheckGameOverCondition(out TileMark? winner);
	void Clear();
}

public interface IGameBoardTileView
{
	bool InputEnabled { get; set; }
	int Row { get; }
	int Col { get; }
	event System.Action<IGameBoardTileView> OnClicked;
}

public interface IGameBoardInputController
{
	event System.Action<IGameBoardTileView> OnTileClicked;
	bool Interactable { get; set; }
	void AttachToButtons();
	void DetatchFromButtons();
}

#endregion

public class GameBoardController
{		
	IGameBoardView view;
	IGameBoardModel model;

	public int Rows { get { return this.model.Rows; } }
	public int Cols { get { return this.model.Cols; } }
	public IGameBoardInputController InputController { get { return this.view.InputController; } }
	public TileMark this[int row, int col] { get { return this.model[row, col]; } }

	public GameBoardController(IGameBoardView view, IGameBoardModel model)
	{
		this.model = model;

		// Initialize View
		this.view = view;
		view.Init(model.Rows, model.Cols);
		view.Refresh(model);
	}

	public void SetTileState(int row, int col, TileMark mark)
	{
		if(this.model.TrySetPlayerMark(row, col, mark))
			this.view.RefreshTile(row, col, mark);
		else
			throw new UnityException(string.Format("Trying to write value {0} to tile [{1}, {2}] which is not empty", mark, row, col));
	}

	public bool CheckGameOverCondition(out TileMark? winner)
	{
		return this.model.CheckGameOverCondition(out winner);
	}

	public void Clear()
	{
		this.model.Clear();
		this.view.Refresh(this.model);
	}
}
