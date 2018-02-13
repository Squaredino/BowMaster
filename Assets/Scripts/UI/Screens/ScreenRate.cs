using DG.Tweening;
using UnityEngine;

public class ScreenRate : MonoBehaviour {
	private void Start()
	{
		transform.localPosition = new Vector3(-1000f, 0f, 0f);
		transform.localScale = new Vector2(0f, 1f);
	}

	private void OnEnable()
	{
		GlobalEvents<OnScreenRateShow>.Happened += OnScreenRateShow;
	}

	private void OnScreenRateShow(OnScreenRateShow obj)
	{
		if (obj.IsBtnClick)
		{
			Show();
		}
		else
		{
			if (PlayerPrefs.GetInt("RateForVersion", -1) == PrefsManager.GameVersion) return;
			int count = PlayerPrefs.GetInt("RateCounter", 0);
			if (count == 3 || count == 10 || count == 25) Show();	
			PlayerPrefs.SetInt("RateCounter", ++count);
		}
	}

	public void Show()
	{
#if UNITY_IOS
		if (Utils.VersionGraterThan(10.3f) && PlayerPrefs.GetInt("RateNativeCounter") < 3)
		{
			PlayerPrefs.SetInt("RateNativeCounter", PlayerPrefs.GetInt("RateNativeCounter") + 1);
			iOSReviewRequest.Request();
			return;
		}
#endif
		transform.DOScaleX(1f, 0.3f).SetEase(Ease.InOutBack);
		transform.localPosition = Vector2.zero;
	}
	
	public void Close()
	{
		transform.DOScaleX(0f, 0.2f).OnComplete(() => { transform.localPosition = new Vector3(-1000f, 0f, 0f); });
	}
}
