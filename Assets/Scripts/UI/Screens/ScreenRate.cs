using DG.Tweening;
using UnityEngine;

public class ScreenRate : MonoBehaviour {
	private void Start()
	{
		transform.localPosition = Vector2.zero;
		transform.localScale = new Vector2(0f, 1f);
	}

	private void OnEnable()
	{
		GlobalEvents<OnScreenRateShow>.Happened += OnScreenRateShow;
	}

	private void OnScreenRateShow(OnScreenRateShow obj)
	{
		if (obj.BtnClick)
		{
#if UNITY_IOS
			if (Utils.VersionGraterThan(10.3f) && PlayerPrefs.GetInt("RateNativeCounter") < 3)
			{
				PlayerPrefs.SetInt("RateNativeCounter", PlayerPrefs.GetInt("RateNativeCounter") + 1);
				iOSReviewRequest.Request();
				return;
			}
#endif

		Show();
	}
	else {
		GlobalEvents<OnRate>.Call(new OnRate());
	}
}

	public void Show()
	{
		transform.DOScaleX(1f, 0.3f).SetEase(Ease.InOutBack);
	}
	
	public void Close()
	{
		transform.DOScaleX(0f, 0.2f);
	}
}
