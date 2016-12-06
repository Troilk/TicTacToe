using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PageGameplayHandlers : UIPage, IPageGameplayHandlers
{
	[Header("UI Elements")]
	[SerializeField, NonNull] Text textResults;
	[SerializeField, NonNull] Text textTurnInfo;
	[SerializeField, NonNull] Text textDifficulty;

	[Header("Texts")]
	[SerializeField] string strYourTurn = "Your Turn - {0}";
	[SerializeField] string strEnemyTurn = "Enemy Turn - {0}";

	[Header("Colors - Turn")]
	[SerializeField] Color colorYourTurn = Color.green;
	[SerializeField] Color colorEnemyTurn = Color.red;

	[Header("Colors - Difficulty")]
	[SerializeField] Color colorEasy = Color.green;
	[SerializeField] Color colorNormal = Color.yellow;
	[SerializeField] Color colorImpossible = Color.red;

	string resultsFormatStr;
	StringBuilder sb = new StringBuilder(128);

	public event System.Action<GameState> OnGameStateTransitionButtonPressed;

	public override void Init(PageType pageType, params UIPage[] pageChildPopupPages)
	{
		base.Init(pageType, pageChildPopupPages);
		// Cache initial results string format
		this.resultsFormatStr = this.textResults.text;
	}

	public void UpdateGameStats(int wins, int losses, int draws, int difficulty)
	{
		this.sb.Length = 0;
		this.sb.AppendFormat(this.resultsFormatStr, wins, losses, draws);
		this.textResults.text = sb.ToString();

		switch(difficulty)
		{
		case 0:
			this.textDifficulty.text = "\n\n\nEasy";
			this.textDifficulty.color = this.colorEasy;
			break;

		case 1:
			this.textDifficulty.text = "\n\n\nNormal";
			this.textDifficulty.color = this.colorNormal;
			break;

		case 2:
			this.textDifficulty.text = "\n\n\nImpossible";
			this.textDifficulty.color = this.colorImpossible;
			break;
		}
	}

	// TODO: connect this stuff
	public void UpdateTurnInfo(TileState playerType, bool userPlayer)
	{
		char playerSymbol = playerType == TileState.Cross ? 'X' : '0';
		this.textTurnInfo.text = string.Format(userPlayer ? this.strYourTurn : this.strEnemyTurn, playerSymbol);
		this.textTurnInfo.color = userPlayer ? this.colorYourTurn : this.colorEnemyTurn;
	}

	#region UI Callbacks

	public void OnButtonMenuPressed()
	{
		if(this.OnGameStateTransitionButtonPressed != null)
			this.OnGameStateTransitionButtonPressed(GameState.Loading);
	}

	#endregion
}
