using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageMainMenuHandlers : UIPage 
{
	[SerializeField, NonNull] ToggleGroup toggleGroupDifficulties;

	int GetSelectedDifficulty()
	{
		IEnumerable<Toggle> activeToggles = this.toggleGroupDifficulties.ActiveToggles();
		int difficulty = activeToggles.GetEnumerator().Current.transform.GetSiblingIndex();
		return difficulty;
	}

	#region UI Callbacks

	public void OnButtonPlayPressed()
	{
		// TODO:
	}

	#endregion
}
