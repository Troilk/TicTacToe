using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PageType
{
	Window,
	Popup,
	Tab
}

[RequireComponent(typeof(CanvasGroup)),
 AddComponentMenu("Pages/UIPage"),
 DisallowMultipleComponent]
public class UIPage : MonoBehaviour
{
	[Tooltip("Content object is containing everything on page except child Popup pages!")]
	[SerializeField, NonNull] protected CanvasGroup content;
	
	// Cached components
	protected GameObject go;
	protected CanvasGroup canvasGroup;
	protected Image faderImage;
	protected ScreenFader fader;

	public PageType Type { get; protected set; }
	protected bool isOpened = false;

	// Related pages
	protected UIPage[] childPopupPages;
	protected UIPage currentPreviousPage = null;
	protected UIPage currentNextPage = null;
	
	protected System.Action dOnClosed = null;
	protected System.Action dOnOpened = null;
	protected System.Action<float> dOnFadeTick = null;
	
	public bool IsOpened { get { return this.isOpened; } }
	
	public virtual void Init(PageType pageType, params UIPage[] pageChildPopupPages)
	{
		this.Type = pageType;

		// preparing canvas group
		this.canvasGroup = this.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0.0f;

		// preparing screen fader with default settings for this page type or custom settings
		CustomScreenFaderSettings customFaderSettings = this.GetComponent<CustomScreenFaderSettings>();
		this.fader = new ScreenFader(customFaderSettings.Settings, this.canvasGroup);

		if(pageType == PageType.Popup)
		{
			this.faderImage = this.GetComponent<Image>();
			this.fader.SetUpPopupBackgroundImage(this.faderImage);
		}
		
		this.go = this.gameObject;
		
		// callbacks
		this.dOnClosed = new System.Action(this.OnClosed);

		// initing child pages
		this.childPopupPages = pageChildPopupPages;
		int childPagesCount = pageChildPopupPages.Length;

		for(int i = 0; i < childPagesCount; ++i)
		{
			pageChildPopupPages[i].Init(PageType.Popup);
		}
	}
	
	public virtual void Open(UIPage previousPage, bool immidiate = false)
	{
		if(this.isOpened)
			return;
		
		this.go.SetActive(true);
		this.currentPreviousPage = previousPage;
		if(this.Type == PageType.Popup && previousPage != null)
			previousPage.Disable();
		this.Enable();
		this.isOpened = true;

		if(immidiate)
		{
			this.fader.ImmidiateFade(true, this.dOnOpened);
			if(this.dOnFadeTick != null)
				this.dOnFadeTick(1.0f);
		}
		else
			this.fader.StartFade(true, this.dOnOpened, this.dOnFadeTick);
	}
	
	public virtual void Close(UIPage nextPage, bool immidiate = false)
	{
		if(!this.isOpened)
			return;
		
		this.isOpened = false;
		this.Disable();
		this.currentNextPage = nextPage;

		if(immidiate)
		{
			this.fader.ImmidiateFade(false, this.dOnClosed);
			if(this.dOnFadeTick != null)
				this.dOnFadeTick(0.0f);
		}
		else
			this.fader.StartFade(false, this.dOnClosed, this.dOnFadeTick);
	}
	
	public virtual void Enable()
	{
		this.content.interactable = true;
	}
	
	public virtual void Disable()
	{
		this.content.interactable = false;
	}

	protected virtual void OnClosedActions()
	{}

	public virtual void OnAndroidBackButtonPressed()
	{
		Debug.LogFormat(this, "Android Back button is not implemeted for {0} page", this.name);
	}

	// callbacks
	void OnClosed()
	{
		this.go.SetActive(false);
		
		if(this.currentNextPage != null)
		{
			this.currentNextPage.Open(null);
			this.currentNextPage = null;
		}
		
		if(this.currentPreviousPage != null)
		{
			this.currentPreviousPage.Enable();
			this.currentPreviousPage = null;
		}

		this.OnClosedActions();
	}

	#region Static Helpers

	public static UIPage[] GetAllPages(bool includeInactive)
	{
		Canvas[] canvases = FindObjectsOfType<Canvas>();
		int canvasesCount = canvases.Length;
		List<UIPage> pages = new List<UIPage>();

		while(--canvasesCount > -1)
			pages.AddRange(canvases[canvasesCount].GetComponentsInChildren<UIPage>(includeInactive));

		return pages.ToArray();
	}

	public static void SetPagesActive(UIPage[] pages, bool setActive)
	{
		int pagesCount = pages.Length;

		for(int i = 0; i < pagesCount; ++i)
		{
			pages[i].gameObject.SetActive(setActive);
		}
	}

	public static void ActivateAllPages()
	{
		SetPagesActive(GetAllPages(true), true);
	}

	#endregion
}
