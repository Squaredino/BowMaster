using System;
using UnityEngine;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
	public static event Action OnClickRevive, OnReviveTimeEnded;
	private float timer;
	[SerializeField] private float _reviveTime;
	[SerializeField] private GameObject _timerLavel;
	[SerializeField] private GameObject _backgroundImage;
	
	private void Start()
	{
		timer = _reviveTime;
		RectTransform rt = GetComponent<RectTransform>();
		rt.localPosition = Vector3.one;
		rt.localScale = Vector2.one;
	}

	public void OnClickReviveButton()
	{
		GameEvents.Send(OnClickRevive);
		Destroy(gameObject);
	}
	
	void Update ()
	{
		if (timer > 0.0f)
		{
			timer -= Time.deltaTime;
//			_timerLavel.GetComponent<Text>().text = Convert.ToInt32(Math.Ceiling(timer)).ToString();
			_backgroundImage.GetComponent<Image>().fillAmount = timer / _reviveTime;

			if (timer <= 0.0f)
			{
				GameEvents.Send(OnReviveTimeEnded);
				Destroy(gameObject);
			}
		}
	}
}
