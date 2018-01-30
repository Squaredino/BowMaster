using DG.Tweening;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	// How long the object should shake for.
	[SerializeField] private float _zoomInTime;
	[SerializeField] private float _zoomOutTime;

	void OnEnable()
	{
		GlobalEvents<OnCameraZoom>.Happened += OnCameraZoom;
	}

	private void OnCameraZoom(OnCameraZoom obj)
	{
		transform.DOMoveZ(obj.ZoomValue, _zoomInTime).OnComplete(() => { transform.DOMoveZ(0f, _zoomOutTime); });
	}
}