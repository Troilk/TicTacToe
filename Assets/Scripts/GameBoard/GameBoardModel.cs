using UnityEngine;

public class WinRow
{
	Point2D[] coords;
	TileMark[,] tiles;

	public TileMark this[Point2D coord] { get { return this.tiles[coord.X, coord.Y]; } }

	public WinRow(TileMark[,] tiles, params Point2D[] coords)
	{
		this.tiles = tiles;
		this.coords = coords;
	}

	public TileMark? GetWinningType()
	{
		TileMark startState = this[this.coords[0]];
		if(startState == TileMark.Empty)
			return null;

		for(int i = 1; i < this.coords.Length; ++i)
		{
			TileMark state = this[this.coords[i]];
			if(state == TileMark.Empty || state != startState)
				return null;
		}

		return startState;
	}
}

public class GameBoardModel : IGameBoardModel
{
	TileMark[,] tiles;
	WinRow[] winRows;
	int emptyTiles;

	public int Rows { get; private set; }
	public int Cols { get; private set; }
	public TileMark this[int row, int col] { get { return this.tiles[row, col]; } }

	public GameBoardModel(int rows, int cols)
	{
		this.Rows = rows;
		this.Cols = cols;

		this.tiles = new TileMark[this.Rows, this.Cols];
		this.emptyTiles = rows * cols;

		// Create 8 win rows (3 rows, 3 cols, 2 diagonals)
		this.GenerateWinRows();
	}

	public bool TrySetPlayerMark(int row, int col, TileMark mark)
	{
		if(this.tiles[row, col] != TileMark.Empty)
			return false;

		--this.emptyTiles;
		this.tiles[row, col] = mark;

		return true;
	}

	public bool CheckGameOverCondition(out TileMark? winner)
	{
		// Check each potential winning row
		for(int i = 0; i < this.winRows.Length; ++i)
		{
			TileMark? type = this.winRows[i].GetWinningType();
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
				this.tiles[row, col] = TileMark.Empty;
		}

		this.emptyTiles = this.Rows * this.Cols;
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
}
