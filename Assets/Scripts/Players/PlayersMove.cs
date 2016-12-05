using UnityEngine;

// Supports up to 255x255 maps
public struct PlayersMove 
{
	public int Row;
	public int Col;

	public int Hash { get { return (this.Row << 8) | this.Col; } }

	public PlayersMove(int row, int col)
	{
		this.Row = row;
		this.Col = col;
	}

	public PlayersMove(int hash) : this(hash >> 8, hash & 0xFF)
	{
	}
}
