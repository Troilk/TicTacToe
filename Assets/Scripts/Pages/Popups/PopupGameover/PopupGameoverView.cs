using UnityEngine;
using UnityEngine.UI;

public class PopupGameoverView : UIPageView
{
	[Header("UI Elements")]
	[SerializeField, NonNull] Image imgWinnerIcon;
	[SerializeField, NonNull] Text textVictory;

	[Header("Sprite")]
	[SerializeField, NonNull] Sprite crossIcon;
	[SerializeField, NonNull] Sprite circleIcon;
	[SerializeField, NonNull] Sprite drawIcon;

	[Header("Texts")]
	[SerializeField] string textDraw = "Draw!";
	[SerializeField] string textWins = "Wins!";

	public event System.Action<GameState> OnGameStateTransitionButtonPressed;
 
	public void SetWinningType(TileMark? winnerType)
	{
		if(winnerType == null)
		{
			this.textVictory.text = this.textDraw;
			this.imgWinnerIcon.sprite = this.drawIcon;
		}
		else
		{
			this.textVictory.text = this.textWins;
			this.imgWinnerIcon.sprite = winnerType.Value == TileMark.Cross ? this.crossIcon : this.circleIcon;
		}
	}

	#region UI Callbacks

	public void OnButtonMenuPressed()
	{
		if(this.OnGameStateTransitionButtonPressed != null)
			this.OnGameStateTransitionButtonPressed(GameState.Loading);
	}

	public void OnButtonPlayPressed()
	{
		if(this.OnGameStateTransitionButtonPressed != null)
			this.OnGameStateTransitionButtonPressed(GameState.Gameplay);
	}

	#endregion
}
