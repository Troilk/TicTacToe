using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button)),
 DisallowMultipleComponent]
public class GameBoardViewDefaultTile : MonoBehaviour, IGameBoardTileView
{
	[System.Serializable]
	public struct TileStateProfile
	{
		[NonNull]
		public Sprite Icon;
		public Color Tint;
		public bool Interactable;

		public TileStateProfile(Sprite sprite, Color tint, bool interactable)
		{
			this.Icon = sprite;
			this.Tint = tint;
			this.Interactable = interactable;
		}
	}

	[SerializeField, NonNull] Image imgTileState;
	Button button;

	public int Row { get; private set; }
	public int Col { get; private set; }

	#region IGameBoardTileView implementation

	public bool InputEnabled { get { return this.button.interactable; } set { this.button.interactable = value; } }
	public event System.Action<IGameBoardTileView> OnClicked;

	#endregion

	public void Init(int row, int col)
	{
		this.Row = row;
		this.Col = col;

		this.name = string.Format("Tile {0}x{1}", row, col);

		this.button = this.GetComponent<Button>();
		this.button.onClick.AddListener(() => {
			if(this.OnClicked != null)
				this.OnClicked(this);
		});
	}

	public void SetTileStateProfile(TileStateProfile profile)
	{
		this.imgTileState.sprite = profile.Icon;
		this.imgTileState.color = profile.Tint;
		this.button.interactable = profile.Interactable;
	}
}