using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Rate : MonoBehaviour
{
    private string _bandleIdAndroid = "com.squaredino.archer";
    private string _bandleIdAppStore = "1344880179";

    private void OnEnable()
    {
        GlobalEvents<OnRate>.Happened += OnRate;
    }

    private void OnRate(OnRate obj)
    {
        RateClick();
    }

    public void RateClick()
    {
        Debug.Log("RateClick");
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + _bandleIdAndroid);
#elif UNITY_IOS
        if (Utils.VersionGraterThan(10.3f) && PlayerPrefs.GetInt("RateNativeCounter") < 3)
        {
            PlayerPrefs.SetInt("RateNativeCounter", PlayerPrefs.GetInt("RateNativeCounter")+1);
            iOSReviewRequest.Request();
        }
        else
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/id" + _bandleIdAppStore);
        }
#endif
        PlayerPrefs.SetInt("RateForVersion", PrefsManager.GameVersion);
        
        Analytics.CustomEvent("RateFeedbackClick",
            new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter}});
    }
}
