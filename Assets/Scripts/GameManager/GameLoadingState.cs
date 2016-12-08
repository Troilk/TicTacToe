using UnityEngine;
using Prime31.StateKit;

public partial class GameManager : MonoBehaviour
{
	private class GameLoadingState : SKState<GameManager> 
	{
		public override void begin()
		{
			_context.hudManager.ChangeGameState(GameState.Loading, null);
		}
	}
}