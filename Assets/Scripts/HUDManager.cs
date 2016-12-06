using UnityEngine;
using System.Collections;

// TODO: rework this stuff so that is uses interfaces ??? don't forget we need interface for UIPage
public interface IPageGameplayHandlers
{
	event System.Action<GameState> OnGameStateTransitionButtonPressed;
	void UpdateGameStats(int wins, int losses, int draws, int difficulty);
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
	PageLoadingHandlers pageLoadingHandlers;

	IGameManager gameManager;
	bool loadingPageShowing = true;

	public event System.Action<GameState> OnHUDRequestGameStateTransition;

	public void Init(IGameManager gameManager)
	{
		if(FindObjectsOfType<HUDManager>().Length > 1)
		{
			Debug.LogError("Multiple HUD managers found in scene", this);
			Destroy(this);
		}

		this.gameManager = gameManager;

		UIPage[] allPages = UIPage.GetAllPages(true);
		UIPage.SetPagesActive(allPages, true);

		this.pageGameplayHandlers = FindObjectOfType<PageGameplayHandlers>();
		this.popupGameoverHandlers = FindObjectOfType<PopupGameoverHandlers>();
		this.pageLoadingHandlers = FindObjectOfType<PageLoadingHandlers>();

		this.pageGameplayHandlers.Init(PageType.Window, this.popupGameoverHandlers);
		this.pageLoadingHandlers.Init(PageType.Window);

		// disable all pages
		UIPage.SetPagesActive(allPages, false);

		gameManager.OnGameStateChanged += this.OnGameStateChanged;
		EventRelay.OnTurnStarted += this.OnTurnStarted;

		this.pageLoadingHandlers.Open(null, true);
	}

	IEnumerator GetLoadingPageCloseRoutine()
	{
		// Wait 1 frame
		yield return null;
		this.pageLoadingHandlers.Close(this.pageGameplayHandlers);
	}

	void OnGameStateChanged(GameState newState, AbstractPlayer winner)
	{
		this.UnsubsribeAllPagesEvents();

		int difficulty = PlayerProfile.PreferredDifficulty;
		this.pageGameplayHandlers.UpdateGameStats(
			PlayerProfile.Wins.GetValue(difficulty),
			PlayerProfile.Losses.GetValue(difficulty),
			PlayerProfile.Draws.GetValue(difficulty),
			difficulty
		);

		switch(newState)
		{
		case GameState.Gameplay:
			this.pageGameplayHandlers.OnGameStateTransitionButtonPressed += this.OnGameStateTransitionButtonPressed;

			if(this.loadingPageShowing)
			{
				this.loadingPageShowing = false;
				StartCoroutine(this.GetLoadingPageCloseRoutine());
			}
			else
			{
				this.pageGameplayHandlers.Open(null);
				this.popupGameoverHandlers.Close(null);
			}
			break;
		case GameState.GameOver:
			this.popupGameoverHandlers.OnGameStateTransitionButtonPressed += this.OnGameStateTransitionButtonPressed;
			this.popupGameoverHandlers.SetWinningType(winner);
			this.popupGameoverHandlers.Open(this.pageGameplayHandlers);
			break;
		case GameState.Loading:
			this.pageLoadingHandlers.SceneToLoad = 0;
			this.pageLoadingHandlers.Open(this.pageGameplayHandlers);
			break;
		}
	}

	void OnGameStateTransitionButtonPressed(GameState targetState)
	{
		if(this.OnHUDRequestGameStateTransition != null)
			this.OnHUDRequestGameStateTransition(targetState);
	}

	void OnTurnStarted(AbstractPlayer player)
	{
		this.pageGameplayHandlers.UpdateTurnInfo(player.Type, player is UserPlayer);
	}

	void UnsubsribeAllPagesEvents()
	{
		this.pageGameplayHandlers.OnGameStateTransitionButtonPressed -= this.OnGameStateTransitionButtonPressed;
		this.popupGameoverHandlers.OnGameStateTransitionButtonPressed -= this.OnGameStateTransitionButtonPressed;
	}

	void OnDestroy()
	{
		this.UnsubsribeAllPagesEvents();
		this.gameManager.OnGameStateChanged -= this.OnGameStateChanged;
		EventRelay.OnTurnStarted -= this.OnTurnStarted;
	}
}
