using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeAdjuster : MonoBehaviour 
{
	[SerializeField] RectTransform targetTransform;
	[SerializeField, Range(0.0f, 100.0f)] float widthPercent = 80.0f;
	[SerializeField, Range(0.0f, 100.0f)] float maxHeightPercent = 70.0f;

	Camera cam;

	void Awake()
	{
		this.cam = this.GetComponent<Camera>();
	}

	// In Update in case window is continuously resized
	void Update()
	{
		// Calculating preferred size
		Vector2 targetSize = this.targetTransform.sizeDelta;
		float targetAspect = targetSize.x / targetSize.y;

		float widthFract = Mathf.Min(this.widthPercent, (this.maxHeightPercent / this.cam.aspect) * targetAspect) * 0.01f;
		float width = targetSize.x / widthFract;

		this.cam.orthographicSize = (width / this.cam.aspect) * 0.5f;
	}
}
