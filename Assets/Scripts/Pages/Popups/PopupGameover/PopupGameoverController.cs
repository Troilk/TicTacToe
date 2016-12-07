using UnityEngine;

public class PopupGameoverController : UIPageController<PopupGameoverView, UIPageModel>
{
	public event System.Action<GameState> OnGameStateTransitionRequested;

	public PopupGameoverController(UIPageModel model, PopupGameoverView view)
		:base(model, view)
	{
		view.OnGameStateTransitionButtonPressed += (targetState) => {
			if(this.OnGameStateTransitionRequested != null)
				this.OnGameStateTransitionRequested(targetState);
		};
	}

	public void SetWinningType(TileMark? winnerType)
	{
		this.view.SetWinningType(winnerType);
	}
}
