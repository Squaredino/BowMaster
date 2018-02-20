using System;
using PrefsEditor;
using UnityEngine;

public class MyAds : MonoBehaviour
{
    public static int NoAds;
    private DateTime _rewardDate;
    private bool _isRewardedWaitTimer;

    private static int _videoAdCounter;
    private static bool _isVideoAdCalcNext;
    private DateTime _videoDate;
    private bool _isVideoWaitTimer;
    private bool _isFirstTimeVideo;

    private void Awake()
    {
        NoAds = SecurePlayerPrefs.GetInt("noAds");
    }

    private void Start()
    {
        _rewardDate = UnbiasedTime.Instance.Now();
        _isRewardedWaitTimer = true;

        _videoDate = UnbiasedTime.Instance.Now();
        _isVideoWaitTimer = true;
        _isVideoAdCalcNext = true;
        _isFirstTimeVideo = true;
        _videoAdCounter = 0;
    }

    void OnEnable()
    {
        GlobalEvents<OnAdsRewardedShowing>.Happened += OnAdsRewardedShowing;
        GlobalEvents<OnAdsVideoTryShow>.Happened += OnAdsVideoTryShow;
        GlobalEvents<OnAdsVideoShowing>.Happened += OnAdsVideoShowing;
        GlobalEvents<OnAdsDisable>.Happened += OnAdsDisable;
        GlobalEvents<OnAdsRewardedBuySkin>.Happened += OnAdsRewardedBuySkin;
    }

    private void OnAdsRewardedBuySkin(OnAdsRewardedBuySkin obj)
    {
        GlobalEvents<OnAdsRewardedShow>.Call(new OnAdsRewardedShow());
        
        //Temp
        GlobalEvents<OnBuySkinByRewarded>.Call(new OnBuySkinByRewarded{Id = obj.Id});
    }

    private void OnAdsDisable(OnAdsDisable obj)
    {
        NoAds = 1;
        SecurePlayerPrefs.SetInt("noAds",1);
    }

    private void OnAdsVideoTryShow(OnAdsVideoTryShow obj)
    {
        Debug.Log("OnAdsVideoTryShow" + _videoAdCounter + " " + _isVideoWaitTimer + " " + _videoDate);
        if (_isFirstTimeVideo && _videoAdCounter == 3 ||
            _videoAdCounter >= 5)
        {
            if (_isFirstTimeVideo) _isFirstTimeVideo = false;

            if (!_isVideoWaitTimer)
            {
                GlobalEvents<OnAdsVideoShow>.Call(new OnAdsVideoShow());
                Debug.Log("GlobalEvents<OnShowVideoAds>");
            }
//            _isVideoAdCalcNext = false;
        }
        else
        {
            if (_isVideoAdCalcNext) ++_videoAdCounter;
        }
    }

    private void OnAdsVideoShowing(OnAdsVideoShowing obj)
    {
        // продолжаем считать геймлпеи, после которых можно показыавть Video рекламу
        StartWaitingVideo();
    }
    
    // Запускаем таймер ожидание следующей Ревардед рекламы
    private void OnAdsRewardedShowing(OnAdsRewardedShowing e)
    {
        _rewardDate = UnbiasedTime.Instance.Now();
        _rewardDate = _rewardDate.AddMinutes(2);
        _isRewardedWaitTimer = true;
        GlobalEvents<OnAdsRewardedWaitTimer>.Call(new OnAdsRewardedWaitTimer {IsWait = true}); 

        //Обнуляем Video таймер и коунтер
        StartWaitingVideo();
    }

    private void StartWaitingVideo()
    {
        _videoDate = UnbiasedTime.Instance.Now();
        _videoDate = _videoDate.AddMinutes(2);
        _videoAdCounter = 1;
        _isVideoAdCalcNext = true;
        _isVideoWaitTimer = true;
    }

    private void Update()
    {
        UpdateVideoTimerEnd();
    }

    private void UpdateVideoTimerEnd()
    {
        if (_isRewardedWaitTimer)
        {
            TimeSpan difference = _rewardDate.Subtract(UnbiasedTime.Instance.Now());
            if (difference.TotalSeconds <= 0f)
            {
                _isRewardedWaitTimer = false;
                GlobalEvents<OnAdsRewardedWaitTimer>.Call(new OnAdsRewardedWaitTimer {IsWait = false}); 
            }
        }
        
        if (_isVideoWaitTimer)
        {
            TimeSpan difference = _videoDate.Subtract(UnbiasedTime.Instance.Now());
            if (difference.TotalSeconds <= 0f)
            {
                _isVideoWaitTimer = false;
            }
        }
    }
}