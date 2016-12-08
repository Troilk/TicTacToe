using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
	[SerializeField] SoundClip btnClickSfx;

	[Header("Page views")]
	[SerializeField, NonNull] PageMainMenuView pageMainMenuView;
	[SerializeField, NonNull] PageLoadingView pageLoadingView;

	PageMainMenuController pageMainMenuController;
	PageLoadingController pageLoadingController;

	void Awake()
	{
		if(FindObjectsOfType<MainMenuController>().Length > 1)
		{
			Debug.LogError("Multiple MainMenuManager managers found in scene", this);
			Destroy(this);
		}

		this.pageMainMenuController = new PageMainMenuController(
			new UIPageModel(PageType.Window),
			this.pageMainMenuView
		);

		this.pageLoadingController = new PageLoadingController(
			new PageLoadingModel(PageType.Window),
			this.pageLoadingView
		);

		SFXHelpers.AddButtonBehaviours(this.pageMainMenuView.gameObject, null, this.btnClickSfx, null, null);
		SFXHelpers.AddButtonBehaviours(this.pageLoadingView.gameObject, null, this.btnClickSfx, null, null);
	}

	void Start()
	{
		this.pageMainMenuController.OnPlayPressed += this.OnPlayPressed;

		// Immidiately show black loading screen
		this.pageLoadingController.OpenAndClose(this.pageMainMenuController);
		System.GC.Collect();
	}

	void OnDestroy()
	{
		this.pageMainMenuController.OnPlayPressed -= this.OnPlayPressed;
	}

	void OnPlayPressed()
	{
		this.pageLoadingController.OpenAndLoadScene(1, this.pageMainMenuController);
	}
}
