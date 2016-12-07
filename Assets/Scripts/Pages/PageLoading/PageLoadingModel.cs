using UnityEngine;
using UnityEngine.SceneManagement;

public class PageLoadingModel : UIPageModel 
{
	int sceneToLoadIdx = -1;

	public PageLoadingModel(PageType type)
		:base(type)
	{}

	public int SceneToLoadIdx {
		get { return this.sceneToLoadIdx; }
		set {
			if(this.sceneToLoadIdx == value)
				return;

			if(value < -1 || value >= SceneManager.sceneCountInBuildSettings)
			{
				Debug.LogErrorFormat("Tried to load scene with unexisting index: {0}", value);
				return;
			}

			this.sceneToLoadIdx = value;
		}
	}
}
