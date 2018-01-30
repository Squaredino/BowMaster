using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	private float shakeDuration;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.7f;
	
	Vector3 originalPos;

	void Awake()
	{  
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
		GlobalEvents<OnCameraShake>.Happened += OnCameraShake;
	}

	private void OnCameraShake(OnCameraShake obj)
	{
		shakeDuration = obj.Time;
		shakeAmount = obj.Value;
	}

	void Update()
	{
		if (shakeDuration <= 0f) return;
		
		if (shakeDuration > 0f)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime;
		}
		else
		{
			shakeDuration = -1f;
			camTransform.localPosition = originalPos;
		}
	}
}