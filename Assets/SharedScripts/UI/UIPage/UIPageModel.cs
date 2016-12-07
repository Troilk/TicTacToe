using UnityEngine;

public enum PageType
{
	Window,
	Popup,
	Tab
}

public enum PageState
{
	Closed = 0,
	Opening,
	Opened,
	Closing
}

public class UIPageModel
{
	PageState pageState = PageState.Closed;

	public event System.Action<PageState> OnPageStateChanged;

	public PageType Type { get; private set; }
	public PageState PageState { 
		get { return this.pageState; }
		set {
			if(this.pageState == value)
				return;

			this.pageState = value;
			if(this.OnPageStateChanged != null)
				this.OnPageStateChanged(value);
		} 
	}
	public bool CanOpen { get { return this.pageState == PageState.Closed || this.pageState == PageState.Closing; } }
	public bool CanClose { get { return this.pageState == PageState.Opened || this.pageState == PageState.Opening; } }

	// Related pages
	public IUIPageController currentPreviousPage = null;
	public IUIPageController currentNextPage = null;

	public UIPageModel(PageType type)
	{
		this.Type = type;
	}
}
