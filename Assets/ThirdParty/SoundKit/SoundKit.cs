using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31.GoKitLite;

public class SoundKit : MonoBehaviour
{
	public static SoundKit instance = null;
	public int initialCapacity = 10;
	public int maxCapacity = 15;
	public bool dontDestroyOnLoad = true;
	public bool clearAllAudioClipsOnLevelLoad = true;
	public SKSound backgroundSound;

	private Stack<SKSound> _availableSounds;
	private List<SKSound> _playingSounds;
	
	private float _soundEffectVolume = 0.9f;
	public float soundEffectVolume
	{
		get { return _soundEffectVolume; }
		set
		{
			_soundEffectVolume = value;

			foreach( var s in _playingSounds )
				s.audioSource.volume = value;
		}
	}


	#region MonoBehaviour

	void Awake()
	{
		// avoid duplicates
		if( instance != null )
		{
			// we set dontDestroyOnLoad to false here due to the Destroy being delayed. it will avoid issues
			// with OnLevelWasLoaded being called while the object is being destroyed.
			dontDestroyOnLoad = false;
			Destroy( gameObject );
			return;
		}

		instance = this;

		if( dontDestroyOnLoad )
			DontDestroyOnLoad( gameObject );

		// Create the _soundList to speed up sound playing in game
		_availableSounds = new Stack<SKSound>( maxCapacity );
		_playingSounds = new List<SKSound>();

		for( int i = 0; i < initialCapacity; i++ )
			_availableSounds.Push( new SKSound( this ) );
	}


	void OnApplicationQuit()
	{
		instance = null;
	}


	void OnLevelWasLoaded( int level )
	{
		if( dontDestroyOnLoad && clearAllAudioClipsOnLevelLoad )
		{
			for( var i = _playingSounds.Count - 1; i >= 0; i-- )
			{
				var s = _playingSounds[i];
				s.audioSource.clip = null;

				_availableSounds.Push( s );
				_playingSounds.RemoveAt( i );
			}
		}
	}


	void Update()
	{
		for( var i = _playingSounds.Count - 1; i >= 0; i-- )
		{
			var sound = _playingSounds[i];
			if( sound._playingLoopingAudio )
				continue;

			sound._elapsedTime += Time.deltaTime;
			if( sound._elapsedTime > sound.audioSource.clip.length )
				sound.stop();
		}
	}

	#endregion


	/// <summary>
	/// fetches the next available sound and adds the sound to the _playingSounds List
	/// </summary>
	/// <returns>The available sound.</returns>
	private SKSound nextAvailableSound()
	{
		SKSound sound = null;

		if( _availableSounds.Count > 0 )
			sound = _availableSounds.Pop();

		// if we didnt find an available found, bail out
		if( sound == null )
			sound = new SKSound( this );
		_playingSounds.Add( sound );

		return sound;
	}


	/// <summary>
	/// starts up the background music and optionally loops it. You can access the SKSound via the backgroundSound field.
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	/// <param name="loop">If set to <c>true</c> loop.</param>
	public void playBackgroundMusic( AudioClip audioClip, float volume, bool loop = true )
	{
		if( backgroundSound == null )
			backgroundSound = new SKSound( this );

		backgroundSound.playAudioClip( audioClip, volume, 1f );
		backgroundSound.setLoop( loop );
	}


	/// <summary>
	/// fetches any AudioSource it can find and uses the standard PlayOneShot to play. Use this if you don't require any
	/// extra control over a clip and don't care about when it completes. It avoids the call to StartCoroutine.
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	/// <param name="volumeScale">Volume scale.</param>
	void playOneShotScaled(AudioClip audioClip, float volumeScale)
	{
		// find an audio source. any will work
		AudioSource source = null;
		
		if( _availableSounds.Count > 0 )
			source = _availableSounds.Peek().audioSource;
		else
			source = _playingSounds[0].audioSource;
		
		source.PlayOneShot(audioClip, volumeScale);
	}

	public void playOneShot(AudioClip audioClip, float volumeScale)
	{
		this.playOneShotScaled(audioClip, volumeScale * _soundEffectVolume);
	}

	public void playOneShot(AudioClip audioClip)
	{
		this.playOneShotScaled(audioClip, _soundEffectVolume);
	}

	/// <summary>
	/// plays the AudioClip with the default volume (soundEffectVolume)
	/// </summary>
	/// <returns>The sound.</returns>
	/// <param name="audioClip">Audio clip.</param>
	public SKSound playSound( AudioClip audioClip )
	{
		return playSound( audioClip, 1.0f );
	}


	/// <summary>
	/// plays the AudioClip with the specified volume
	/// </summary>
	/// <returns>The sound.</returns>
	/// <param name="audioClip">Audio clip.</param>
	/// <param name="volume">Volume.</param>
	public SKSound playSound( AudioClip audioClip, float volume )
	{
		return playSound( audioClip, volume, 1f );
	}
	
	
	/// <summary>
	/// plays the AudioClip with the specified pitch
	/// </summary>
	/// <returns>The sound.</returns>
	/// <param name="audioClip">Audio clip.</param>
	/// <param name="pitch">Pitch.</param>
	public SKSound playPitchedSound( AudioClip audioClip, float pitch )
	{
		return playSound( audioClip, 1.0f, pitch );
	}


	/// <summary>
	/// plays the AudioClip with the specified volume and pitch
	/// </summary>
	/// <returns>The sound.</returns>
	/// <param name="audioClip">Audio clip.</param>
	/// <param name="volume">Volume.</param>
	public SKSound playSound( AudioClip audioClip, float volume, float pitch )
	{
		// Find the first SKSound not being used. if they are all in use, create a new one
		SKSound sound = nextAvailableSound();
		sound.playAudioClip( audioClip, volume * _soundEffectVolume, pitch );

		return sound;
	}


	/// <summary>
	/// loops the AudioClip. Do note that you are responsible for calling either stop or fadeOutAndStop on the SKSound
	/// or it will not be recycled
	/// </summary>
	/// <returns>The sound looped.</returns>
	/// <param name="audioClip">Audio clip.</param>
	public SKSound playSoundLooped( AudioClip audioClip, float volumeScale )
	{
		// find the first SKSound not being used. if they are all in use, create a new one
		SKSound sound = nextAvailableSound();
		sound.playAudioClip( audioClip, volumeScale * _soundEffectVolume, 1f );
		sound.setLoop( true );

		return sound;
	}


	/// <summary>
	/// used internally to recycle SKSounds and their AudioSources
	/// </summary>
	/// <param name="sound">Sound.</param>
	public void recycleSound( SKSound sound )
	{
		var index = 0;
		while( index < _playingSounds.Count )
		{
			if( _playingSounds[index] == sound )
				break;
			++index;
		}
		_playingSounds.RemoveAt( index );


		// if we are already over capacity dont recycle this sound but destroy it instead
		if( _availableSounds.Count + _playingSounds.Count >= maxCapacity )
			Destroy( sound.audioSource );
		else
			_availableSounds.Push( sound );
	}


	#region SKSound inner class

	public class SKSound
	{
		private SoundKit _manager;

		public AudioSource audioSource;
		internal Action _completionHandler;
		internal bool _playingLoopingAudio;
		internal float _elapsedTime;

		// fade
		internal float _fadeStartVolume;
		internal float _fadeTargetVolume;
		internal Action<Transform, float> _dFade;
		internal int _fadeLastTweenID = -1;

		// fade pitch
		internal float _fadeStartPitch;
		internal float _fadeTargetPitch;
		internal Action<Transform, float> _dFadePitch;
		internal int _fadePitchLastTweenID = -1;
		//internal Coroutine _currentCoroutine;

		public SKSound( SoundKit manager )
		{
			_manager = manager;
			audioSource = _manager.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}


		/// <summary>
		/// fades out the audio over duration. this will short circuit and stop immediately if the elapsedTime exceeds the clip.length
		/// </summary>
		/// <returns>The out.</returns>
		/// <param name="duration">Duration.</param>
		/// <param name="onComplete">On complete.</param>
		private IEnumerator fadeOut( float duration, Action onComplete )
		{
			var startingVolume = audioSource.volume;
			duration = 1f / duration;

			// fade out the volume
			while( audioSource.volume > 0.0f && _elapsedTime < audioSource.clip.length )
			{
				audioSource.volume -= Time.deltaTime * startingVolume * duration;
				yield return null;
			}

			stop();

			// all done fading out
			if( onComplete != null )
				onComplete();
		}

//		private IEnumerator c_fade(float duration, float targetVolume, Action onComplete)
//		{
//			float fadeVolume = targetVolume - this.audioSource.volume;
//			float minVol, maxVol;
//			if(targetVolume > this.audioSource.volume)
//			{
//				minVol = this.audioSource.volume;
//				maxVol = targetVolume;
//			}
//			else
//			{
//				minVol = targetVolume;
//				maxVol = this.audioSource.volume;
//			}
//			duration = 1f / duration;
//
//			//fade in the volume
//			while(this.audioSource.volume != targetVolume && this._elapsedTime < this.audioSource.clip.length)
//			{
//				this.audioSource.volume = Mathf.Clamp(this.audioSource.volume + Time.unscaledDeltaTime * fadeVolume * duration, minVol, maxVol);
//				yield return null;
//			}
//
//			_currentCoroutine = null;
//			if(onComplete != null)
//				onComplete();
//		}


		/// <summary>
		/// sets whether the SKSound should loop. If true, you are responsible for calling stop on the SKSound to recycle it!
		/// </summary>
		/// <returns>The SKSound.</returns>
		/// <param name="shouldLoop">If set to <c>true</c> should loop.</param>
		public SKSound setLoop( bool shouldLoop )
		{
			_playingLoopingAudio = true;
			audioSource.loop = shouldLoop;

			return this;
		}


		/// <summary>
		/// sets an Action that will be called when the clip finishes playing
		/// </summary>
		/// <returns>The SKSound.</returns>
		/// <param name="handler">Handler.</param>
		public SKSound setCompletionHandler( Action handler )
		{
			_completionHandler = handler;

			return this;
		}


		/// <summary>
		/// stops the audio clip, fires the completionHandler if necessary and recycles the SKSound
		/// </summary>
		public void stop()
		{
			audioSource.Stop();

			if( _completionHandler != null )
			{
				_completionHandler();
				_completionHandler = null;
			}

			if(this != this._manager.backgroundSound)
				_manager.recycleSound( this );
		}


		/// <summary>
		/// fades out the audio clip over time. Note that if the clip finishes before the fade completes it will short circuit
		/// the fade and stop playing
		/// </summary>
		/// <param name="duration">Duration.</param>
		/// <param name="handler">Handler.</param>
		public void fadeOutAndStop( float duration, Action handler = null )
		{
			_manager.StartCoroutine
			(
				fadeOut( duration, () =>
			    {
					if( handler != null )
						handler();
				})
			);
		}

		void FadeActionFunc(Transform transform, float easedTime)
		{
			this.audioSource.volume = Mathf.LerpUnclamped(this._fadeStartVolume, this._fadeTargetVolume, easedTime);
		}

		public void fade(float duration, float targetVolume, Func<float, float, float> easingFunc, Action<Transform> handler = null)
		{
			if(duration == 0.0f)
			{
				// stop previous tween
				GoKitLite.instance.stopTween(this._fadeLastTweenID, false);
				this._fadeLastTweenID = -1;
				this.audioSource.volume = targetVolume;
				return;
			}

			if(this._dFade == null)
				this._dFade = new Action<Transform, float>(this.FadeActionFunc);

			int lastTweenId = this._fadeLastTweenID;

			this._fadeLastTweenID = GoKitLite.instance.customAction(this._manager.transform, duration, this._dFade)
				.setEaseFunction(easingFunc)
				.setTimeScaleIndependent()
				.setCompletionHandler(handler)
				.getId();
			this._fadeStartVolume = this.audioSource.volume;
			this._fadeTargetVolume = targetVolume;

			// stop currently working fade
			GoKitLite.instance.stopTween(lastTweenId, false);
		}

		void FadePitchActionFunc(Transform transform, float easedTime)
		{
			this.audioSource.pitch = Mathf.LerpUnclamped(this._fadeStartPitch, this._fadeTargetPitch, easedTime);
		}

		public void fadePitch(float duration, float targetPitch, Func<float, float, float> easingFunc, Action<Transform> handler = null)
		{
			if(duration == 0.0f)
			{
				// stop previous tween
				GoKitLite.instance.stopTween(this._fadePitchLastTweenID, false);
				this._fadePitchLastTweenID = -1;
				this.audioSource.pitch = targetPitch;
				return;
			}

			if(this._dFadePitch == null)
				this._dFadePitch = new Action<Transform, float>(this.FadePitchActionFunc);

			int lastTweenId = this._fadePitchLastTweenID;

			this._fadePitchLastTweenID = GoKitLite.instance.customAction(this._manager.transform, duration, this._dFadePitch)
				.setEaseFunction(easingFunc)
				.setTimeScaleIndependent()
				.setCompletionHandler(handler)
				.getId();
			this._fadeStartPitch = this.audioSource.pitch;
			this._fadeTargetPitch = targetPitch;

			// stop currently working fade
			GoKitLite.instance.stopTween(lastTweenId, false);
		}

		internal void playAudioClip( AudioClip audioClip, float volume, float pitch )
		{
			_playingLoopingAudio = false;
			_elapsedTime = 0;

			// setup the GameObject and AudioSource and start playing
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			// reset some defaults in case the AudioSource was changed
			audioSource.loop = false;
			audioSource.panStereo = 0;
			audioSource.mute = false;

			audioSource.Play();
		}

	}

	#endregion

}
