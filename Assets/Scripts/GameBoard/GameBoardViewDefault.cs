using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas)),
 DisallowMultipleComponent]
public class GameBoardViewDefault : MonoBehaviour, GameBoard.IGameBoardView
{
	[SerializeField, NonNull] GridLayoutGroup gridGroup;
	[SerializeField, NonNull] GameBoardViewDefaultTile tilePrefab;

	[SerializeField] GameBoardViewDefaultTile.TileStateProfile crossProfile = new GameBoardViewDefaultTile.TileStateProfile(null, Color.blue, false);
	[SerializeField] GameBoardViewDefaultTile.TileStateProfile circleProfile = new GameBoardViewDefaultTile.TileStateProfile(null, Color.red, false);

	GameBoard board;
	GameBoardViewDefaultTile[,] tiles;
	GameBoardViewDefaultTile.TileStateProfile emptyProfile = new GameBoardViewDefaultTile.TileStateProfile(null, Color.clear, true);

	public GameBoard.IGameBoardInputController InputController { get; private set; }

	public void Init(GameBoard board)
	{
		this.board = board;

		// Creating Images to display board
		int rows = board.Rows;
		int cols = board.Cols;
		this.tiles = new GameBoardViewDefaultTile[rows, cols];

		this.gridGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		this.gridGroup.constraintCount = cols;

		this.gridGroup.gameObject.DestroyAllChildren();

		for(int row = 0; row < rows; ++row)
		{
			for(int col = 0; col < cols; ++col)
			{
				var tile = Instantiate<GameBoardViewDefaultTile>(this.tilePrefab);
				tile.transform.SetParent(this.gridGroup.transform);
				tile.transform.SetSiblingIndex(col + row * cols);
				tile.Init(row, col);
				tile.SetTileStateProfile(this.emptyProfile);

				this.tiles[row, col] = tile;
			}
		}

		// Creating Input Controller
		this.InputController = new GameBoardInputController(this.gridGroup.GetComponent<CanvasGroup>(),
			Utility.Convert2DArrayTo1D<GameBoardViewDefaultTile>(this.tiles));
		this.InputController.AttachToButtons();
	}

	public void RefreshTile(int row, int col)
	{
		// Select sprite based on current state of tile
		TileState state = this.board[row, col];
		var profile = state == TileState.Cross ? this.crossProfile : (state == TileState.Circle ? this.circleProfile : this.emptyProfile);

		this.tiles[row, col].SetTileStateProfile(profile);
	}

	public void Refresh()
	{
		int rows = this.board.Rows;
		int cols = this.board.Cols;

		for(int row = 0; row < rows; ++row)
		{
			for(int col = 0; col < cols; ++col)
			{
				this.RefreshTile(row, col);
			}
		}
	}
}
