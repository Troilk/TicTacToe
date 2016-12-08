using UnityEngine;
using Prime31.StateKit;

public partial class GameManager : MonoBehaviour
{
	private class GameplayState : SKState<GameManager>
	{
		public override void begin()
		{
			_context.turnManager.OnGameOver += this.OnGameOver;
			_context.hudManager.OnHUDRequestGameStateTransition += this.OnHUDRequestGameStateTransition;

			_context.gameBoard.Clear();
			_context.turnManager.Start();

			_context.hudManager.ChangeGameState(GameState.Gameplay, null);
		}

		public override void end()
		{
			_context.turnManager.OnGameOver -= this.OnGameOver;
			_context.hudManager.OnHUDRequestGameStateTransition -= this.OnHUDRequestGameStateTransition;
		}

		void OnHUDRequestGameStateTransition(GameState targetState)
		{
			if(targetState != GameState.Loading)
			{
				Debug.LogError("Game can transition only to LoadingState from GameplayState. Tried to transition to " + targetState);
				return;
			}

			_machine.changeState<GameLoadingState>();
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

			_machine.changeState<GameoverState>();
			_context.hudManager.ChangeGameState(GameState.GameOver, winner);

			this.PlayGameOverSFX(winner);
		}

		void PlayGameOverSFX(AbstractPlayer winner)
		{
			if(winner == null)
				_context.sfxDraw.Play();
			else if(winner is IUserControlledPlayer)
				_context.sfxVictory.Play();
			else
				_context.sfxDefeat.Play();
		}
	}
}