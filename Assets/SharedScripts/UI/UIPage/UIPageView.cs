using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(CustomScreenFaderSettings)),
 DisallowMultipleComponent]
public class UIPageView : MonoBehaviour
{
	[Tooltip("Content object is containing everything on page except child Popup pages!")]
	[SerializeField, NonNull] protected CanvasGroup content;

	CanvasGroup canvasGroup;
	public ScreenFader Fader { get; private set; }

	public virtual void Init(PageType pageType)
	{
		// Required for GetComponent to work
		this.gameObject.SetActive(true);

		// preparing canvas group
		this.canvasGroup = this.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0.0f;

		// preparing screen fader with default settings for this page type or custom settings
		CustomScreenFaderSettings customFaderSettings = this.GetComponent<CustomScreenFaderSettings>();
		this.Fader = new ScreenFader(customFaderSettings.Settings, this.canvasGroup);

		if(pageType == PageType.Popup)
		{
			Image faderImage = this.GetComponent<Image>();
			this.Fader.SetUpPopupBackgroundImage(faderImage);
		}

		this.gameObject.SetActive(false);
	}

	public virtual void Enable()
	{
		this.content.interactable = true;
	}

	public virtual void Disable()
	{
		this.content.interactable = false;
	}
}
