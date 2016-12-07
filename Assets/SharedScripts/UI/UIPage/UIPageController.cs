using UnityEngine;

public interface IUIPageController
{
	bool Open(IUIPageController previousPage, bool immidiate = false);
	bool Close(IUIPageController nextPage, bool immidiate = false);
	void Enable();
	void Disable();
}

public class UIPageController<View, Model> : IUIPageController 
	where View 	: UIPageView
	where Model : UIPageModel
{
	protected Model model;
	protected View view;

	protected event System.Action<float> OnFadeTick;

	public UIPageController(Model model, View view)
	{
		this.model = model;
		this.view = view;

		view.Init(model.Type);
		model.OnPageStateChanged += OnPageStateChanged;
	}

	public virtual bool Open(IUIPageController previousPage, bool immidiate = false)
	{
		if(!this.model.CanOpen) 
			return false;

		this.model.currentPreviousPage = previousPage;
		if(this.model.Type == PageType.Popup && previousPage != null)
			previousPage.Disable();
		this.model.PageState = PageState.Opening;
		
		this.view.gameObject.SetActive(true);
		this.Enable();

		// Start fading in the page
		ScreenFader fader = this.view.Fader;
		if(immidiate)
		{
			fader.ImmidiateFade(true, () => this.model.PageState = PageState.Opened);
			if(this.OnFadeTick != null)
				this.OnFadeTick(1.0f);
		}
		else
			fader.StartFade(true, () => this.model.PageState = PageState.Opened, this.OnFadeTick);

		return true;
	}

	public virtual bool Close(IUIPageController nextPage, bool immidiate = false)
	{
		if(!this.model.CanClose)
			return false;

		this.model.PageState = PageState.Closing;
		this.model.currentNextPage = nextPage;

		this.Disable();

		// Start fading out the page
		ScreenFader fader = this.view.Fader;
		if(immidiate)
		{
			fader.ImmidiateFade(false, () => this.model.PageState = PageState.Closed);
			if(this.OnFadeTick != null)
				this.OnFadeTick(0.0f);
		}
		else
			fader.StartFade(false, () => this.model.PageState = PageState.Closed, this.OnFadeTick);

		return true;
	}

	public virtual void Enable()
	{
		this.view.Enable();
	}

	public virtual void Disable()
	{
		this.view.Disable();
	}

	protected virtual void OnPageStateChanged(PageState newState)
	{
		if(newState != PageState.Closed)
			return;
		
		this.view.gameObject.SetActive(false);

		if(this.model.currentNextPage != null)
		{
			this.model.currentNextPage.Open(null);
			this.model.currentNextPage = null;
		}

		if(this.model.currentPreviousPage != null)
		{
			this.model.currentPreviousPage.Enable();
			this.model.currentPreviousPage = null;
		}
	}
}
