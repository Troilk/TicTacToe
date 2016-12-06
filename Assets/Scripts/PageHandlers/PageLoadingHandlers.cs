using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageLoadingHandlers : UIPage
{
	public int SceneToLoad { get; set; }

	public override void Init(PageType pageType, params UIPage[] pageChildPopupPages)
	{
		base.Init(pageType, pageChildPopupPages);

		this.SceneToLoad = -1;
		this.dOnOpened = () => {
			if(this.SceneToLoad != -1)
				SceneManager.LoadScene(this.SceneToLoad);
		};
	}
}
