using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPageMainMenuHandlers
{
	event System.Action<int> OnPlayPressed;
}

public class MainMenuManager : MonoBehaviour 
{
	PageMainMenuView pageMainMenuHandlers;
	PageLoadingHandlers pageLoadingHandlers;

	void Awake()
	{
		if(FindObjectsOfType<MainMenuManager>().Length > 1)
		{
			Debug.LogError("Multiple MainMenuManager managers found in scene", this);
			Destroy(this);
		}

		UIPage[] allPages = UIPage.GetAllPages(true);
		UIPage.SetPagesActive(allPages, true);

		// Cache references to pages
		this.pageMainMenuHandlers = FindObjectOfType<PageMainMenuView>();
		this.pageLoadingHandlers = FindObjectOfType<PageLoadingHandlers>();

		// Init pages
		this.pageMainMenuHandlers.Init(PageType.Window);
		this.pageLoadingHandlers.Init(PageType.Window);

		// disable all pages
		UIPage.SetPagesActive(allPages, false);
	}

	void Start()
	{
		// Immidiately show black loading screen
		this.pageLoadingHandlers.Open(null, true);
		System.GC.Collect();
		StartCoroutine(this.GetLoadingPageCloseRoutine());
	}

	IEnumerator GetLoadingPageCloseRoutine()
	{
		// Wait 1 frame
		yield return null;

		this.pageMainMenuHandlers.OnPlayPressed += this.OnPlayPressed;
		this.pageMainMenuHandlers.SetSelectedDifficulty(PlayerProfile.PreferredDifficulty);
		this.pageLoadingHandlers.Close(this.pageMainMenuHandlers);
	}

	void OnDestroy()
	{
		this.pageMainMenuHandlers.OnPlayPressed -= this.OnPlayPressed;
	}

	void OnPlayPressed(int difficulty)
	{
		PlayerProfile.SetPreferredDifficulty(difficulty);
		this.pageLoadingHandlers.SceneToLoad = 1;
		this.pageLoadingHandlers.Open(this.pageMainMenuHandlers);
	}
}
