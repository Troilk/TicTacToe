using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageLoadingController : UIPageController<PageLoadingView, PageLoadingModel>
{
	public PageLoadingController(PageLoadingModel model, PageLoadingView view)
		: base(model, view)
	{}

	public void OpenAndLoadScene(int sceneToLoadIdx, IUIPageController previousPage)
	{
		this.model.SceneToLoadIdx = sceneToLoadIdx;
		this.Open(previousPage);
	}

	public void OpenAndClose(IUIPageController nextPage)
	{
		this.model.SceneToLoadIdx = -1;
		this.Open(null, true);
		this.view.StartCoroutine(this.GetLoadingPageCloseRoutine(nextPage));
	}

	// There is some lag happening after first frame of scene loading,
	// so this coroutine starts page fade out after 1 frame
	IEnumerator GetLoadingPageCloseRoutine(IUIPageController nextPage)
	{
		// Wait 1 frame
		yield return null;
		this.Close(nextPage);
	}

	protected override void OnPageStateChanged(PageState newState)
	{
		base.OnPageStateChanged(newState);
		if(newState == PageState.Opened && this.model.SceneToLoadIdx != -1)
			SceneManager.LoadScene(this.model.SceneToLoadIdx);
	}
}
