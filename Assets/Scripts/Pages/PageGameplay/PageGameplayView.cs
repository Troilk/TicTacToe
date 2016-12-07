using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PageGameplayView : UIPageView
{
	[System.Serializable]
	private struct PageGameplayViewDifficultyProfile
	{
		public string Title;
		public Color Color;

		public PageGameplayViewDifficultyProfile(string title, Color color)
		{
			this.Title = title;
			this.Color = color;
		}
	}

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

	[Header("Difficulty display settings")]
	[SerializeField] PageGameplayViewDifficultyProfile easyProfile = new PageGameplayViewDifficultyProfile("Easy", Color.green);
	[SerializeField] PageGameplayViewDifficultyProfile normalProfile = new PageGameplayViewDifficultyProfile("Normal", Color.yellow);
	[SerializeField] PageGameplayViewDifficultyProfile impossibleProfile = new PageGameplayViewDifficultyProfile("Impossible", Color.red);

	string resultsFormatStr;
	StringBuilder sb = new StringBuilder(128);

	public event System.Action<GameState> OnGameStateTransitionButtonPressed;

	public override void Init(PageType pageType)
	{
		base.Init(pageType);
		// Cache initial results string format
		this.resultsFormatStr = this.textResults.text;
	}

	public void UpdateGameStats(int wins, int losses, int draws, int difficulty)
	{
		this.sb.Length = 0;
		this.sb.AppendFormat(this.resultsFormatStr, wins, losses, draws);
		this.textResults.text = sb.ToString();

		var profile = difficulty == 0 ? this.easyProfile : 
			(difficulty == 1 ? this.normalProfile : this.impossibleProfile);

		this.textDifficulty.text = "\n\n\n" + profile.Title;
		this.textDifficulty.color = profile.Color;
	}
		
	public void UpdateTurnInfo(TileMark playerType, bool userPlayer)
	{
		char playerSymbol = playerType == TileMark.Cross ? 'X' : '0';
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
