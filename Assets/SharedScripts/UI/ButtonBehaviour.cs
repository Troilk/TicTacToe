using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("UI Components/ButtonBehaviour")]
public class ButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
	// Sounds
	[Header("Active SFX")]
	public SoundClip PressedSound;
	public SoundClip ReleasedSound;

	[Header("Disabled SFX")]
	public SoundClip PressedSoundDisabled;
	public SoundClip ReleasedSoundDisabled;
	
	// Cached components
	Selectable selectable;
	
	void Awake()
	{
		this.selectable = this.GetComponent<Selectable>();
		this.selectable.navigation = new Navigation(){ mode = Navigation.Mode.None };
	}
	
	public virtual void OnPointerDown(PointerEventData e)
	{
		if(this.selectable.IsInteractable())
		{
			if(this.PressedSound != null)
				this.PressedSound.Play();
		}
		else
		{
			if(this.PressedSoundDisabled != null)
				this.PressedSoundDisabled.Play();
		}
	}
	
	public virtual void OnPointerClick(PointerEventData e)
	{
		if(this.selectable.IsInteractable())
		{
			if(this.ReleasedSound != null)
				this.ReleasedSound.Play();
		}
		else
		{
			if(this.ReleasedSoundDisabled != null)
				this.ReleasedSoundDisabled.Play();
		}
	}
}
