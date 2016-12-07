using UnityEngine;

public enum GameState
{
	Loading = 0,
	Gameplay,
	GameOver,
	GamePaused
}

public interface IGameManager
{
	event System.Action<GameState, AbstractPlayer> OnGameStateChanged;
}

// TODO: does LINQ statements work on mobile devices
// TODO: What should happen if we exit to menu - we reset turn info ??
public class GameManager : MonoBehaviour, IGameManager
{
	const int ROWS = 3;
	const int COLS = 3;

	[SerializeField, NonNull] GameBoardViewDefault boardView;
	[SerializeField, NonNull] HUDController hudManager;

	GameBoardController gameBoard;
	TurnManager turnManager;

	public GameState GameState { get; private set; }
	public event System.Action<GameState, AbstractPlayer> OnGameStateChanged;

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

		this.hudManager.Init(this);
		this.hudManager.OnHUDRequestGameStateTransition += this.OnHUDRequestGameStateTransition;
	}

	void Start()
	{
		this.ChangeGameState(GameState.Gameplay);
	}

	void OnDestroy()
	{
		this.hudManager.OnHUDRequestGameStateTransition -= OnHUDRequestGameStateTransition;
		this.turnManager.OnGameOver -= this.OnGameOver;

		EventRelay.ResetEvents();
	}

	void OnGameOver(AbstractPlayer winner)
	{
		int difficulty = PlayerProfile.PreferredDifficulty;

		// Update player stats
		if(winner == null)
			PlayerProfile.Draws.IncrementValue(difficulty);
		else if(winner is IUserControlledPlayer)
			PlayerProfile.Wins.IncrementValue(difficulty);
		else
			PlayerProfile.Losses.IncrementValue(difficulty);

		this.ChangeGameState(GameState.GameOver, winner);
	}

	void OnHUDRequestGameStateTransition(GameState targetState)
	{
		if(targetState == GameState.GameOver)
		{
			Debug.LogWarning("HUD is not allowed to transition game to GameOver state");
			return;
		}

		this.ChangeGameState(targetState);
	}

	void ChangeGameState(GameState targetState, AbstractPlayer winner = null)
	{
		if(this.GameState == targetState)
			return;

		this.GameState = targetState;

		if(targetState == GameState.Gameplay)
		{
			this.gameBoard.Clear();

			this.turnManager.OnGameOver += this.OnGameOver;
			this.turnManager.Start();
		}
		else
		{
			this.turnManager.OnGameOver -= this.OnGameOver;
		}

		Debug.LogFormat("<color=green>Game State changed to {0}</color>", this.GameState);
		if(this.OnGameStateChanged != null)
			this.OnGameStateChanged(targetState, winner);
	}
}
