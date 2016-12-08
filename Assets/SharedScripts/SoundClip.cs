using UnityEngine;

[System.Serializable]
public class SoundClip
{
	[SerializeField] AudioClip clip;
	[SerializeField] float volumeScale = 1.0f;

	public AudioClip Clip { get { return this.clip; } }
	public float VolumeScale { get { return this.volumeScale; } }

	public void Play()
	{
		if(this.clip != null)
			SoundKit.instance.playOneShot(this.clip, this.volumeScale);
		#if DEBUG
		else
			Debug.Log("<color=lightblue>Tried to play clip with no AudioClip</color>");
		#endif
	}
}