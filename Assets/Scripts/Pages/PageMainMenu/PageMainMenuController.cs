using UnityEngine;

public class PageMainMenuController : UIPageController<PageMainMenuView, UIPageModel>
{
	public event System.Action OnPlayPressed;

	public PageMainMenuController(UIPageModel model, PageMainMenuView view)
		: base(model, view)
	{
		this.view.SetSelectedDifficulty(PlayerProfile.PreferredDifficulty);

		this.view.OnPlayPressed += (difficulty) => { 
			PlayerProfile.SetPreferredDifficulty(difficulty);
			if(this.OnPlayPressed != null)
				this.OnPlayPressed();
		};
	}
}
