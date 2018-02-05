using System;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
	public static event Action OnPointerDown; 
	public static event Action OnPointerUp; 
	public static event Action OnPointerMove; 
	private Image _image;
	
	void Start ()
	{
		_image = GetComponent<Image>();
		GlobalEvents<OnGameInputEnable>.Happened += OnGameInputEnable;
	}

	private void OnGameInputEnable(OnGameInputEnable obj)
	{
		_image.raycastTarget = obj.Flag;
	}

	public void PointerDown()
	{
		GameEvents.Send(OnPointerDown);
	}
	
	public void PointerUp()
	{
		GameEvents.Send(OnPointerUp);
	}
	
	public void PointerMove()
	{
		GameEvents.Send(OnPointerMove);
	}
}
