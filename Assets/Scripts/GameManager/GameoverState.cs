using UnityEngine;
using Prime31.StateKit;

public partial class GameManager : MonoBehaviour
{
	private class GameoverState : SKState<GameManager> 
	{
		public override void begin()
		{
			_context.hudManager.OnHUDRequestGameStateTransition += this.OnHUDRequestGameStateTransition;
		}

		public override void end()
		{
			_context.hudManager.OnHUDRequestGameStateTransition -= this.OnHUDRequestGameStateTransition;
		}

		void OnHUDRequestGameStateTransition(GameState targetState)
		{
			if(targetState != GameState.Loading && targetState != GameState.Gameplay)
			{
				Debug.LogError("Game can transition only to LoadingState or GameplayState from GameoverState. Tried to transition to " + targetState);
				return;
			}

			if(targetState == GameState.Loading)
				_machine.changeState<GameLoadingState>();
			else
				_machine.changeState<GameplayState>();
		}
	}
}