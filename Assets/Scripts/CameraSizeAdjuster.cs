using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeAdjuster : MonoBehaviour 
{
	[SerializeField] RectTransform targetTransform;
	[SerializeField, Range(0.0f, 100.0f)] float widthPercent = 80.0f;

	Camera cam;

	void Awake()
	{
		this.cam = this.GetComponent<Camera>();
	}

	// In Update in case window is continuously resized
	void Update()
	{
		float width = this.targetTransform.sizeDelta.x;
		width /= (this.widthPercent * 0.01f);

		this.cam.orthographicSize = (width / this.cam.aspect) * 0.5f;
	}
}
