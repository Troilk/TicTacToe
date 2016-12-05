using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PageMainMenuHandlers : UIPage, IPageMainMenuHandlers
{
	[SerializeField, NonNull] ToggleGroup toggleGroupDifficulties;
	Toggle[] toggles;

	public event System.Action<int> OnPlayPressed;

	public override void Init(PageType pageType, params UIPage[] pageChildPopupPages)
	{
		base.Init(pageType, pageChildPopupPages);
		this.toggles = this.toggleGroupDifficulties.GetComponentsInChildren<Toggle>(true);
	}

	public void SetSelectedDifficulty(int difficulty)
	{
		this.toggleGroupDifficulties.SetAllTogglesOff();
		int togglesCount = this.toggles.Length;

		for(int i = 0; i < togglesCount; ++i)
			this.toggles[i].isOn = i == difficulty;
	}

	int GetSelectedDifficulty()
	{
		IEnumerable<Toggle> activeToggles = this.toggleGroupDifficulties.ActiveToggles();
		Toggle toggle = null;
		if(activeToggles != null)
			toggle = activeToggles.FirstOrDefault<Toggle>();

		if(toggle == null)
		{
			Debug.LogError("No difficulty toggle seems to be selected");
			return 0;
		}

		return toggle.transform.GetSiblingIndex();
	}

	#region UI Callbacks

	public void OnButtonPlayPressed()
	{
		if(this.OnPlayPressed != null)
			this.OnPlayPressed(this.GetSelectedDifficulty());
	}

	#endregion
}
