using UnityEngine;
using Prime31.StateKit;

public enum GameState
{
	Loading = 0,
	Gameplay,
	GameOver
}

// TODO: does LINQ statements work on mobile devices
// TODO: What should happen if we exit to menu - we reset turn info ??
public partial class GameManager : MonoBehaviour
{
	const int ROWS = 3;
	const int COLS = 3;

	[SerializeField, NonNull] GameBoardViewDefault boardView;
	[SerializeField, NonNull] HUDController hudManager;

	[Header("SFX")]
	[SerializeField] SoundClip sfxVictory;
	[SerializeField] SoundClip sfxDefeat;
	[SerializeField] SoundClip sfxDraw;

	GameBoardController gameBoard;
	TurnManager turnManager;
	SKStateMachine<GameManager> fsm;

	void Awake()
	{
		// Creating Game Board
		this.gameBoard = new GameBoardController(this.boardView, new GameBoardModel(ROWS, COLS));

		// Creating Players
		var userPlayer = new UserPlayer(gameBoard, TileMark.Cross);
		// Use different AI player settings based on selected difficulty
		AbstractPlayer aiPlayer;
		if(PlayerProfile.PreferredDifficulty == 0)
			aiPlayer = new RandomAIPlayer(gameBoard, TileMark.Circle);
		else
			aiPlayer = new MinMaxAIPlayer(gameBoard, TileMark.Circle, PlayerProfile.PreferredDifficulty == 1 ? 0.3f : 0.0f);

		this.turnManager = new TurnManager(gameBoard, userPlayer, aiPlayer);
		this.hudManager.Init();

		// Initializing State Machine
		this.fsm = new SKStateMachine<GameManager>(this, new GameplayState());
		this.fsm.addState(new GameoverState());
		this.fsm.addState(new GameLoadingState());
#if DEBUG
		this.fsm.onStateChanged += () => {
			Debug.LogFormat("<color=green>GameManager FSM state changed to {0}</color>", this.fsm.currentState);
		};
#endif
	}

	void OnDestroy()
	{
		EventRelay.ResetEvents();
	}
}
