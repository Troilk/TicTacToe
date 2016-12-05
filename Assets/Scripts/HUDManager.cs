using UnityEngine;

// TODO: rework this stuff so that is uses interfaces ??? don't forget we need interface for UIPage
public interface IPageGameplayHandlers
{
	event System.Action<GameState> OnGameStateTransitionButtonPressed;
	void UpdateGameStats(int wins, int losses, int draws);
}

public interface IPopupGameoverHandlers
{
	event System.Action<GameState> OnGameStateTransitionButtonPressed;
	void SetWinningType(AbstractPlayer winner);
}

public class HUDManager : MonoBehaviour
{
	PageGameplayHandlers pageGameplayHandlers;
	PopupGameoverHandlers popupGameoverHandlers;

	public event System.Action<GameState> OnHUDRequestGameStateTransition;

	public void Init(IGameManager gameManager)
	{
		if(FindObjectsOfType<HUDManager>().Length > 1)
		{
			Debug.LogError("Multiple HUD managers found in scene", this);
			Destroy(this);
		}

		UIPage[] allPages = UIPage.GetAllPages(true);
		UIPage.SetPagesActive(allPages, true);

		this.pageGameplayHandlers = FindObjectOfType<PageGameplayHandlers>();
		this.popupGameoverHandlers = FindObjectOfType<PopupGameoverHandlers>();

		this.pageGameplayHandlers.Init(PageType.Window, this.popupGameoverHandlers);

		// disable all pages
		UIPage.SetPagesActive(allPages, false);

		gameManager.OnGameStateChanged += this.OnGameStateChanged;
	}

	void OnGameStateChanged(GameState newState, AbstractPlayer winner)
	{
		this.UnsubsribeAllEvents();
		this.pageGameplayHandlers.UpdateGameStats(PlayerProfile.Wins, PlayerProfile.Losses, PlayerProfile.Draws);

		switch(newState)
		{
		case GameState.Gameplay:
			this.pageGameplayHandlers.OnGameStateTransitionButtonPressed += this.OnGameStateTransitionButtonPressed;
			this.pageGameplayHandlers.Open(null);
			this.popupGameoverHandlers.Close(null);
			break;
		case GameState.GameOver:
			this.popupGameoverHandlers.OnGameStateTransitionButtonPressed += this.OnGameStateTransitionButtonPressed;
			this.popupGameoverHandlers.SetWinningType(winner);
			this.popupGameoverHandlers.Open(this.pageGameplayHandlers);
			break;
		}
	}

	void OnGameStateTransitionButtonPressed(GameState targetState)
	{
		if(this.OnHUDRequestGameStateTransition != null)
			this.OnHUDRequestGameStateTransition(targetState);
	}

	void UnsubsribeAllEvents()
	{
		this.pageGameplayHandlers.OnGameStateTransitionButtonPressed -= this.OnGameStateTransitionButtonPressed;
		this.popupGameoverHandlers.OnGameStateTransitionButtonPressed -= this.OnGameStateTransitionButtonPressed;
	}

	void OnDestroy()
	{
		this.UnsubsribeAllEvents();
	}
}
