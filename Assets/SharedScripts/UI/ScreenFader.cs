using UnityEngine;
using UnityEngine.UI;
using Prime31.GoKitLite;

public class ScreenFader
{
	[System.Serializable]
	public class ScreenFaderSettings
	{
		[SerializeField] GoEaseType easeType = GoEaseType.Linear;
		[SerializeField] float fadeTime = 0.3f;
		[SerializeField] Color color = Color.black;
		
		public GoEaseType EaseType { get { return this.easeType; } }
		public float FadeTime { get { return this.fadeTime; } }
		public Color Color { get { return this.color; } }
	}

	Transform tr;
	CanvasGroup canvasGroup;
	ScreenFaderSettings settings;

	float m_Alpha = 0.0f;
	// fading in or fading out window/popup
	bool isFadingIn = true;
	int currentTweenId;
	// value from which current lerp tween started (not always 0.0f or 1.0f)
	float currentFromLerp = 0.0f;

	System.Action<float> OnTickExternal = null;
	System.Action OnCompleteExternal = null;

	System.Action<Transform, float> dOnTick;
	System.Action<Transform> dOnComplete;

	// gets/sets alpha + alpha of canvas group if one is assigned
	public float Alpha {
		get { return this.m_Alpha; }
		private set 
		{ 
			this.m_Alpha = value;
			if(this.canvasGroup != null)
				this.canvasGroup.alpha = value;
		} 
	}

	public ScreenFader(ScreenFaderSettings settings, CanvasGroup canvasGroup)
	{
		this.canvasGroup = canvasGroup;
		this.tr = canvasGroup.transform;
		this.settings = settings;

		// caching callbacks
		this.dOnTick = new System.Action<Transform, float>(this.OnTick);
		this.dOnComplete = new System.Action<Transform>(this.OnComplete);
	}

	#region Standard callbacks

	void OnTick(Transform tranf, float t)
	{
		this.Alpha = Mathf.LerpUnclamped(this.currentFromLerp, this.isFadingIn ? 1.0f : 0.0f, t);
		if(this.OnTickExternal != null)
			this.OnTickExternal(this.Alpha);
	}

	void OnComplete(Transform transf)
	{
		this.currentTweenId = -1;
		if(this.OnCompleteExternal != null)
			this.OnCompleteExternal();
	}

	#endregion

	public void ImmidiateFade(bool fadeIn, System.Action onFadeCompleted)
	{
		Utility.CheckStopTween(ref this.currentTweenId, true);

		//Camera.main.backgroundColor = this.Color;
		this.Alpha = fadeIn ? 1.0f : 0.0f;

		if(onFadeCompleted != null)
			onFadeCompleted();
	}

	public void StartFade(bool fadeIn,
	                      System.Action onFadeCompleted,
	                      System.Action<float> onFadeTick)
	{
		// stop previous tween if it was running
		Utility.CheckStopTween(ref this.currentTweenId, true);

		// calculate fade time (if we not go full range from 0 to 1 (currently not in fully faded state), we will just fade for fraction of time)
		this.currentFromLerp = this.Alpha;
		float fadeTime = this.settings.FadeTime * (fadeIn ? 1.0f - this.currentFromLerp : this.currentFromLerp);

		// start new tween
		this.currentTweenId = GoKitLite.instance.customAction(this.tr, fadeTime, this.dOnTick)
			.setEaseFunction(GoKitHelpers.EaseFunctionForType(this.settings.EaseType))
			.setTimeScaleIndependent(true)
			.setCompletionHandler(this.dOnComplete)
			.getId();

		// set custom tween tick/completion callbacks
		this.OnCompleteExternal = onFadeCompleted;
		this.OnTickExternal = onFadeTick;

		this.isFadingIn = fadeIn;
		//Camera.main.backgroundColor = this.Color;
	}

	public void SetUpPopupBackgroundImage(Image backgroundImage)
	{
		backgroundImage.enabled = true;
		backgroundImage.color = this.settings.Color;
	}
}
