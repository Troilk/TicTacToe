using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PageGameplayHandlers : UIPage, IPageGameplayHandlers
{
	[Header("UI Elements")]
	[SerializeField, NonNull] Text textResults;
	[SerializeField, NonNull] Text textTurnInfo;

	[Header("Texts")]
	[SerializeField] string strYourTurn = "Your Turn - {0}";
	[SerializeField] string strEnemyTurn = "Enemy Turn - {1}";

	string resultsFormatStr;
	StringBuilder sb = new StringBuilder(128);

	public event System.Action<GameState> OnGameStateTransitionButtonPressed;

	public override void Init(PageType pageType, params UIPage[] pageChildPopupPages)
	{
		base.Init(pageType, pageChildPopupPages);
		// Cache initial results string format
		this.resultsFormatStr = this.textResults.text;
	}

	public void UpdateGameStats(int wins, int losses, int draws)
	{
		this.sb.Length = 0;
		this.sb.AppendFormat(this.resultsFormatStr, wins, losses, draws);
		this.textResults.text = sb.ToString();
	}

	// TODO: connect this stuff
	public void UpdateTurnInfo(TileState playerType, bool userPlayer)
	{
		char playerSymbol = playerType == TileState.Cross ? 'X' : '0';
		this.textTurnInfo.text = string.Format(userPlayer ? this.strYourTurn : this.strEnemyTurn, playerSymbol);
	}

	#region UI Callbacks

	public void OnButtonMenuPressed()
	{
		if(this.OnGameStateTransitionButtonPressed != null)
			this.OnGameStateTransitionButtonPressed(GameState.Loading);
	}

	#endregion
}
