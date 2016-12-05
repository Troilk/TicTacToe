using UnityEngine;
using UnityEngine.SceneManagement;

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
// TODO: probably rework static Instance
// TODO: Win, Loss counts must be per difficulty
// TODO: What should happen if we exit to menu - we reset turn info ??
public class GameManager : MonoBehaviour, IGameManager
{
	const int ROWS = 3;
	const int COLS = 3;

	// TODO: need to implement through interface somehow...maybe it should be fabric...or load via Resources
	[SerializeField, NonNull] GameBoardViewDefault boardView;
	[SerializeField, NonNull] HUDManager hudManager;

	// TODO: probably should not hold references to these
	GameBoard gameBoard;
	TurnManager turnManager;

	public GameState GameState { get; private set; }
	public event System.Action<GameState, AbstractPlayer> OnGameStateChanged;

	void Awake()
	{
		// Creating Game Board
		this.gameBoard = new GameBoard(this.boardView, ROWS, COLS);

		// Creating Players
		var userPlayer = new UserPlayer(gameBoard, TileState.Cross);
		// TODO: use different AI player settings based on selected difficulty
		var aiPlayer = new RandomAIPlayer(gameBoard, TileState.Circle);

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
	}

	void OnGameOver(AbstractPlayer winner)
	{
		// Update player stats
		if(winner == null)
			PlayerProfile.AddDraw();
		else if(winner is IUserControlledPlayer)
			PlayerProfile.AddWin();
		else
			PlayerProfile.AddLoss();

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
		else if(targetState == GameState.Loading)
		{
			SceneManager.LoadScene(0);
		}
		else
		{
			this.turnManager.OnGameOver -= this.OnGameOver;
		}

		Debug.LogFormat("<color=red>Game State changed to {0}</color>", this.GameState);
		if(this.OnGameStateChanged != null)
			this.OnGameStateChanged(targetState, winner);
	}
}
