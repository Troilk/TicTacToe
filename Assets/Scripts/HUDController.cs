using UnityEngine;

[DisallowMultipleComponent]
public class HUDController : MonoBehaviour
{
	// Page View references
	[SerializeField, NonNull] PageGameplayView 	pageGameplayView;
	[SerializeField, NonNull] PopupGameoverView popupGameoverView;
	[SerializeField, NonNull] PageLoadingView 	pageLoadingView;

	// Page controllers
	PageGameplayController 	pageGameplayController;
	PopupGameoverController	popupGameoverController;
	PageLoadingController 	pageLoadingController;

	IGameManager gameManager;

	public event System.Action<GameState> OnHUDRequestGameStateTransition;

	public void Init(IGameManager gameManager)
	{
		if(FindObjectsOfType<HUDController>().Length > 1)
		{
			Debug.LogError("Multiple HUD managers found in scene", this);
			Destroy(this);
		}

		this.gameManager = gameManager;
		gameManager.OnGameStateChanged += this.OnGameStateChanged;
		EventRelay.OnTurnStarted += this.OnTurnStarted;

		this.CreatePageControllers();

		this.pageLoadingController.OpenAndClose(this.pageGameplayController);
	}

	void CreatePageControllers()
	{
		this.pageGameplayController = new PageGameplayController(
			new UIPageModel(PageType.Window),
			this.pageGameplayView
		);

		this.popupGameoverController = new PopupGameoverController(
			new UIPageModel(PageType.Popup),
			this.popupGameoverView
		);

		this.pageLoadingController = new PageLoadingController(
			new PageLoadingModel(PageType.Window),
			this.pageLoadingView
		);
	}

	void OnGameStateChanged(GameState newState, AbstractPlayer winner)
	{
		this.UnsubsribeAllPagesEvents();
		this.pageGameplayController.UpdateGameStats();

		switch(newState)
		{
		case GameState.Gameplay:
			this.pageGameplayController.OnGameStateTransitionRequested += this.OnGameStateTransitionRequested;
			this.pageGameplayController.Open(null);
			this.popupGameoverController.Close(null);
			break;
		case GameState.GameOver:
			this.popupGameoverController.OnGameStateTransitionRequested += this.OnGameStateTransitionRequested;
			this.popupGameoverController.SetWinningType(winner == null ? (TileMark?)null : winner.Type);
			this.popupGameoverController.Open(this.pageGameplayController);
			break;
		case GameState.Loading:
			this.pageLoadingController.OpenAndLoadScene(0, this.pageGameplayController);
			break;
		}
	}

	void OnGameStateTransitionRequested(GameState targetState)
	{
		if(this.OnHUDRequestGameStateTransition != null)
			this.OnHUDRequestGameStateTransition(targetState);
	}

	void OnTurnStarted(AbstractPlayer player)
	{
		this.pageGameplayController.UpdateTurnInfo(player.Type, player is IUserControlledPlayer);
	}

	void UnsubsribeAllPagesEvents()
	{
		this.pageGameplayController.OnGameStateTransitionRequested -= this.OnGameStateTransitionRequested;
		this.popupGameoverController.OnGameStateTransitionRequested -= this.OnGameStateTransitionRequested;
	}

	void OnDestroy()
	{
		this.UnsubsribeAllPagesEvents();
		this.gameManager.OnGameStateChanged -= this.OnGameStateChanged;
		EventRelay.OnTurnStarted -= this.OnTurnStarted;
	}
}
