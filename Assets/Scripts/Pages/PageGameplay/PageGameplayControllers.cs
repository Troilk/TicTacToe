using UnityEngine;

public class PageGameplayController : UIPageController<PageGameplayView, UIPageModel> 
{
	public event System.Action<GameState> OnGameStateTransitionRequested;

	public PageGameplayController(UIPageModel model, PageGameplayView view)
		:base(model, view)
	{
		view.OnGameStateTransitionButtonPressed += (targetState) => {
			if(this.OnGameStateTransitionRequested != null)
				this.OnGameStateTransitionRequested(targetState);
		};
	}

	public void UpdateGameStats()
	{
		int difficulty = PlayerProfile.PreferredDifficulty;

		this.view.UpdateGameStats(
			PlayerProfile.Wins.GetValue(difficulty),
			PlayerProfile.Losses.GetValue(difficulty),
			PlayerProfile.Draws.GetValue(difficulty),
			difficulty);
	}

	public void UpdateTurnInfo(TileMark playerType, bool userPlayer)
	{
		this.view.UpdateTurnInfo(playerType, userPlayer);
	}
}
