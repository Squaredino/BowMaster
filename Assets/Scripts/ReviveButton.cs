using System;
using UnityEngine;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
	public static event Action OnRevive, OnReviveTimeEnded;
	
	[SerializeField] private float _reviveTime;
//	[SerializeField] private GameObject _timerLevel;
	[SerializeField] private GameObject _backgroundImage;

	private float _time;
	
	private void Start()
	{
		_time = _reviveTime;
		RectTransform rt = GetComponent<RectTransform>();
		rt.localPosition = Vector3.one;
		rt.localScale = Vector2.one;
	}

	private void OnEnable()
	{
		GlobalEvents<OnGiveReward>.Happened += OnGiveReward;
	}
	
	private void OnDisable()
	{
		GlobalEvents<OnGiveReward>.Happened -= OnGiveReward;
	}

	private void OnGiveReward(OnGiveReward obj)
	{
		GameEvents.Send(OnRevive);
	}

	public void OnClickReviveButton()
	{
#if UNITY_EDITOR
		GameEvents.Send(OnRevive);
#else 
	GlobalEvents<OnAdsRewardedShow>.Call(new OnAdsRewardedShow());
#endif
		
		Destroy(gameObject);
	}
	
	void Update ()
	{
		if (_time > 0.0f)
		{
			_time -= Time.deltaTime;
//			_timerLevel.GetComponent<Text>().text = Convert.ToInt32(Math.Ceiling(timer)).ToString();
			_backgroundImage.GetComponent<Image>().fillAmount = _time / _reviveTime;

			if (_time <= 0.0f)
			{
				GameEvents.Send(OnReviveTimeEnded);
				Destroy(gameObject);
			}
		}
	}
}
