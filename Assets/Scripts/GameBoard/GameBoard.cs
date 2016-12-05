using UnityEngine;

public enum TileState
{
	Empty = 0,
	Cross,
	Circle
}

public class GameBoard
{
	#region Interfaces

	public interface IGameBoardView
	{
		void Init(GameBoard board);
		void RefreshTile(int row, int col);
		void Refresh();
		IGameBoardInputController InputController { get; }
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
		event System.Action<GameBoard.IGameBoardTileView> OnTileClicked;
		bool Interactable { get; set; }
		void AttachToButtons();
		void DetatchFromButtons();
	}

	#endregion

	public class WinRow
	{
		Point2D[] coords;
		TileState[,] tiles;

		public TileState this[Point2D coord] { get { return this.tiles[coord.X, coord.Y]; } }

		public WinRow(TileState[,] tiles, params Point2D[] coords)
		{
			this.tiles = tiles;
			this.coords = coords;
		}

		public TileState? GetWinningType()
		{
			TileState startState = this[this.coords[0]];
			if(startState == TileState.Empty)
				return null;

			for(int i = 1; i < this.coords.Length; ++i)
			{
				TileState state = this[this.coords[i]];
				if(state == TileState.Empty || state != startState)
					return null;
			}

			return startState;
		}
	}
		
	TileState[,] tiles;
	WinRow[] winRows;
	IGameBoardView view;
	int emptyTiles;

	public IGameBoardInputController InputController { get { return this.view.InputController; } }
	public int Rows { get; private set; }
	public int Cols { get; private set; }
	public TileState this[int row, int col] { get { return this.tiles[row, col]; } }

	public GameBoard(IGameBoardView view, int rows, int cols)
	{
		this.Rows = rows;
		this.Cols = cols;

		this.tiles = new TileState[this.Rows, this.Cols];
		this.emptyTiles = rows * cols;

		// Create 8 win rows (3 rows, 3 cols, 2 diagonals)
		this.GenerateWinRows();

		// Create GameBoard View
		this.view = view;
		view.Init(this);
		view.Refresh();
	}

	void GenerateWinRows()
	{
		this.winRows = new WinRow[8];

		for(int i = 0; i < 3; ++i)
		{
			// Rows
			this.winRows[i] = new WinRow(this.tiles, new Point2D(i, 0), new Point2D(i, 1), new Point2D(i, 2));

			// Cols
			this.winRows[i + 3] = new WinRow(this.tiles, new Point2D(0, i), new Point2D(1, i), new Point2D(2, i));
		}

		this.winRows[6] = new WinRow(this.tiles, new Point2D(0, 0), new Point2D(1, 1), new Point2D(2, 2));
		this.winRows[7] = new WinRow(this.tiles, new Point2D(2, 0), new Point2D(1, 1), new Point2D(0, 2));
	}

	public void SetTileState(int row, int col, TileState state)
	{
		if(this.tiles[row, col] != TileState.Empty)
		{
			Debug.LogErrorFormat(null, "Trying to write value {0} to tile [{1}, {2}] which is not empty", state, row, col);
			return;
		}

		--this.emptyTiles;
		this.tiles[row, col] = state;
		this.view.RefreshTile(row, col);
	}

	public bool CheckGameOverCondition(out TileState? winner)
	{
		// Check each potential winning row
		for(int i = 0; i < this.winRows.Length; ++i)
		{
			TileState? type = this.winRows[i].GetWinningType();
			if(type != null)
			{
				winner = type;
				return true;
			}
		}

		winner = null;
		return this.emptyTiles == 0;
	}

	public void Clear()
	{
		for(int row = 0; row < this.Rows; ++row)
		{
			for(int col = 0; col < this.Cols; ++col)
				this.tiles[row, col] = TileState.Empty;
		}

		this.emptyTiles = this.Rows * this.Cols;
		this.view.Refresh();
	}
}
