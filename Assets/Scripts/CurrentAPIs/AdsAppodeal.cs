using System.Collections;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdsAppodeal : MonoBehaviour, IInterstitialAdListener, IRewardedVideoAdListener, IBannerAdListener, IPermissionGrantedListener 
{
    #if UNITY_ANDROID
        private string appKey = "";
    #elif UNITY_IPHONE
		private string appKey = "4e02acd2ad213d506caa820a1b317735ae842e83491d22ff";
	#else
		private string appKey = "";
	#endif
    
    public bool testingMode;
    public bool loggingMode;

    private void Awake()
    {
        #if UNITY_ANDROID
           Appodeal.requestAndroidMPermissions(this);
	    #endif
    }
    
    private void Start()
    {
        Appodeal.setLogLevel(loggingMode ? Appodeal.LogLevel.Verbose : Appodeal.LogLevel.None);
        Appodeal.setTesting(testingMode);
        
        Appodeal.disableLocationPermissionCheck();
        Appodeal.disableWriteExternalStoragePermissionCheck();
        
        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO | Appodeal.BANNER);
        
        Appodeal.setInterstitialCallbacks (this);
        Appodeal.setRewardedVideoCallbacks(this);
        Appodeal.setBannerCallbacks(this);
    }
    
    
    #region Standart staff
    private void OnApplicationFocus(bool hasFocus) 
    {
        Debug.Log("Focus = " + hasFocus);
        
        if (hasFocus) 
        {
            Appodeal.onResume();
        }
    }
    #endregion
    
    private void OnEnable() {
		GlobalEvents<OnAdsVideoShow>.Happened += ShowVideo;
		GlobalEvents<OnAdsRewardedShow>.Happened += ShowRewarded;
		GlobalEvents<OnAdsBannerShow>.Happened += OnAdsBannerShow;
	}

    private void ShowVideo(OnAdsVideoShow obj)
    {
        if (!ShowInterstitial())
        {
            onInterstitialClosed();
        }
    }
    
    private void ShowRewarded(OnAdsRewardedShow obj)
    {
        if (!ShowRewardedVideo())
        {
            onRewardedVideoClosed(false);
        }
    }
    
    private void OnAdsBannerShow(OnAdsBannerShow obj)
    {
        StartCoroutine (ShowBanner());
    }

    // INTERSTITIAL ===================================================================================================

    public static bool ShowInterstitial()
    {
        return Appodeal.show(Appodeal.INTERSTITIAL);
    }
    
    #region Interstitial callback handlers
    
    public void onInterstitialLoaded(bool isPrecache) { print("Interstitial loaded"); }
    public void onInterstitialFailedToLoad() { print("Interstitial failed to load"); }
    public void onInterstitialShown() { print("Interstitial shown"); }
    public void onInterstitialClicked() { print("Interstitial clicked"); }

    public void onInterstitialClosed()
    {
        print("Interstitial closed");
        GlobalEvents<OnAdsVideoClosed>.Call(new OnAdsVideoClosed());
    }

    #endregion
    
    // REWARDED VIDEO =================================================================================================

    public static bool ShowRewardedVideo()
    {
        if (!Appodeal.canShow(Appodeal.REWARDED_VIDEO)) return false;
        return Appodeal.show(Appodeal.REWARDED_VIDEO);
    }
    
    #region Rewarded Video callback handlers
    
    public void onRewardedVideoLoaded() { print("Rewarded video loaded"); }
    public void onRewardedVideoFailedToLoad() { print("Rewarded video failed to load"); }
    public void onRewardedVideoShown() { print("Rewarded video shown"); }
    public void onRewardedVideoFinished(int amount, string name) { print("Rewarded video finished adn reward amount = " + amount + ", name = " + name); }
    
    public void onRewardedVideoClosed(bool finished)
    {
        print("Rewarded video closed and finished = " + finished );
        GlobalEvents<OnAdsRewardedClosed>.Call(new OnAdsRewardedClosed{IsReward = finished});
    }

    #endregion
    
    // BANNER =========================================================================================================

    private IEnumerator ShowBanner() {
        while (true) {
            if (Appodeal.isLoaded(Appodeal.BANNER_BOTTOM))
            {
                if (Appodeal.show(Appodeal.BANNER_BOTTOM)) yield break;
            }

            yield return new WaitForSeconds (3f);
        }
    }
    
    private void HideBottomBanner()
    {
        Appodeal.hide(Appodeal.BANNER_BOTTOM);
    }
    
    #region Banner callback handlers
    
    public void onBannerLoaded(bool precache) { print("banner loaded"); }
    public void onBannerFailedToLoad() { print("banner failed"); }
    public void onBannerShown() { print("banner opened"); }
    public void onBannerClicked() { print("banner clicked"); }
    
    #endregion

    // PERMISSIONS ====================================================================================================
    #region Permission Grant callback handlers
    
    public void writeExternalStorageResponse(int result) 
    { 
        if (result == 0) 
        {
            Debug.Log("WRITE_EXTERNAL_STORAGE permission granted"); 
        }
        else
        {
            Debug.Log("WRITE_EXTERNAL_STORAGE permission grant refused"); 
        }
    }
    
    public void accessCoarseLocationResponse(int result) 
    { 
        if(result == 0) 
        {
            Debug.Log("ACCESS_COARSE_LOCATION permission granted"); 
        }
        else
        {
            Debug.Log("ACCESS_COARSE_LOCATION permission grant refused"); 
        }
    }
    
    #endregion
}
