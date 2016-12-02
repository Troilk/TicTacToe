using UnityEngine;

public class CustomScreenFaderSettings : MonoBehaviour
{
	[SerializeField] ScreenFader.ScreenFaderSettings settings;
	public ScreenFader.ScreenFaderSettings Settings { get { return this.settings; } } 
} 
