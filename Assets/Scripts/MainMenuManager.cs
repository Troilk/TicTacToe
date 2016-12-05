using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPageMainMenuHandlers
{
	event System.Action<int> OnPlayPressed;
}

public class MainMenuManager : MonoBehaviour 
{
	PageMainMenuHandlers pageMainMenuHandlers;

	void Awake()
	{
		if(FindObjectsOfType<MainMenuManager>().Length > 1)
		{
			Debug.LogError("Multiple MainMenuManager managers found in scene", this);
			Destroy(this);
		}

		UIPage[] allPages = UIPage.GetAllPages(true);
		UIPage.SetPagesActive(allPages, true);

		this.pageMainMenuHandlers = FindObjectOfType<PageMainMenuHandlers>();
		this.pageMainMenuHandlers.Init(PageType.Window);

		// disable all pages
		UIPage.SetPagesActive(allPages, false);
	}

	void Start()
	{
		this.pageMainMenuHandlers.OnPlayPressed += this.OnPlayPressed;
		this.pageMainMenuHandlers.SetSelectedDifficulty(PlayerProfile.PreferredDifficulty);
		this.pageMainMenuHandlers.Open(null);
	}

	void OnDestroy()
	{
		this.pageMainMenuHandlers.OnPlayPressed -= this.OnPlayPressed;
	}

	void OnPlayPressed(int difficulty)
	{
		PlayerProfile.SetPreferredDifficulty(difficulty);
		SceneManager.LoadScene(1);
	}
}
