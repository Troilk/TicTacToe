using UnityEngine;
using UnityEngine.UI;

public static class SFXHelpers
{
	public static void AddButtonBehaviours(GameObject parent,
		SoundClip sfxPressed, SoundClip sfxReleased,
		SoundClip sfxPressedDisabled, SoundClip sfxReleasedDisabled)
	{
		foreach(var btn in parent.GetComponentsInChildren<Selectable>(true))
		{
			var btnBehaviour = btn.gameObject.AddComponent<ButtonBehaviour>();

			btnBehaviour.PressedSound = sfxPressed;
			btnBehaviour.ReleasedSound = sfxReleased;
			btnBehaviour.PressedSoundDisabled = sfxPressedDisabled;
			btnBehaviour.ReleasedSoundDisabled = sfxReleasedDisabled;
		}
	}
}
