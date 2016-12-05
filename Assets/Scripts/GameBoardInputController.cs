using UnityEngine;
using UnityEngine.UI;

public class GameBoardInputController : GameBoard.IGameBoardInputController
{
	CanvasGroup tileButtonsRoot;
	GameBoard.IGameBoardTileView[] tiles;

	public event System.Action<GameBoard.IGameBoardTileView> OnTileClicked;
	public bool Interactable { get { return this.tileButtonsRoot.interactable; } set { 
			this.tileButtonsRoot.interactable = value; 
		} 
	}

	public GameBoardInputController(CanvasGroup tileButtonsRoot, GameBoard.IGameBoardTileView[] tiles)
	{
		this.tileButtonsRoot = tileButtonsRoot;
		this.tiles = tiles;

		this.Interactable = false;
	}

	public void AttachToButtons()
	{
		int tilesCount = this.tiles.Length;
		for(int i = 0; i < tilesCount; ++i)
			this.tiles[i].OnClicked += this.OnTileClickedHandler;
	}

	public void DetatchFromButtons()
	{
		int tilesCount = this.tiles.Length;
		for(int i = 0; i < tilesCount; ++i)
			this.tiles[i].OnClicked -= this.OnTileClickedHandler;
	}
		
	void OnTileClickedHandler(GameBoard.IGameBoardTileView tile)
	{
		if(this.OnTileClicked != null)
			this.OnTileClicked(tile);
	}
}
