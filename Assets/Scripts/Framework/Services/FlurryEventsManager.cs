using UnityEngine;

public class FlurryEventsManager : MonoBehaviour
{
//    private static float realTimeOnEnterBackground;
//    private static readonly float realTimeOnExitBackground = 0f;
//
//    public static bool dontSendLengthtEvent = false;
//
//    private static bool startScreenLengthOpened = false;
//    private static bool attemptLength = false;
//    private static bool iapShopLengthOpened = false;
//    private static bool candyShopOpened = false;
//
//    private readonly bool coldSessionStarted = false;


    private void Awake()
    {
        //PublishingService.Instance.OnSceneTransitionShown += OnSceneTransitionShown;
    }

    private void OnDestroy()
    {
        //PublishingService.Instance.OnSceneTransitionShown -= OnSceneTransitionShown;
    }

    private void OnSceneTransitionShown()
    {
        AdShow();
    }

    public static void AdShow()
    {
        //FlurryEvent flurryEvent = new FlurryEvent("ad_show");
        //flurryEvent.AddParameter("game_time", GetTimeTotalInMin());
        //FlurryEvents.LogEvent(flurryEvent);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        /*if (pauseStatus)
        {
            //if (!dontSendLengthtEvent) {
                if (startScreenLengthOpened)
                    FlurryEventsManager.SendEndEvent ("start_screen_length", true);
                if (attemptLength)
                    FlurryEventsManager.SendEndEvent ("attempt_length", true);
                if (iapShopLengthOpened)
                    FlurryEventsManager.SendEndEvent ("iap_shop_length", true);
                if (candyShopOpened)
                    FlurryEventsManager.SendEndEvent ("candy_shop_length", true);
            //}

            //if (dontSendLengthtEvent)
            //	dontSendLengthtEvent = false;

            OnEnterBackground();
        }
        else
        {
            //if (!dontSendLengthtEvent) {
                if (startScreenLengthOpened)
                    FlurryEventsManager.SendStartEvent ("start_screen_length");
                if (attemptLength)
                    FlurryEventsManager.SendStartEvent ("attempt_length");
                if  (iapShopLengthOpened)
                    FlurryEventsManager.SendStartEvent ("iap_shop_length");
                if (candyShopOpened)
                    FlurryEventsManager.SendStartEvent ("candy_shop_length");
            //}

            //if (dontSendLengthtEvent)
            //	dontSendLengthtEvent = false;

            OnExitBackground();
        }*/
    }

    private void OnApplicationQuit()
    {
        /*if (Application.platform == RuntimePlatform.Android) 
        {
            OnEnterBackground();
        } else {
            SendEndSessionEvent ();
        }*/
    }

//    private void OnEnterBackground()
//    {
//        Debug.Log("OnEnterBackground");
//        realTimeOnEnterBackground = Time.realtimeSinceStartup;
//        var realSessionTime = realTimeOnEnterBackground - realTimeOnExitBackground;
//        SetRealSessionTime(realSessionTime);
//    }

    private void OnExitBackground()
    {
        /*Debug.Log ("OnExitBackground");
        realTimeOnExitBackground = Time.realtimeSinceStartup;

        float realBackgroundTime = realTimeOnExitBackground - realTimeOnEnterBackground;
        if (realBackgroundTime >= 3600f) 
        {
            if (GetRealSessionTime() > 0) SendEndSessionEvent();
            ResetRealSessionTime();

            SendStartSesionEvent();
        }*/
    }

//    private void Update()
//    {
//        if (!coldSessionStarted)
//        {
//            if (FlurryEvents.Analytics != null)
//            {
//                if (GetRealSessionTime() > 0f){
//                    SendEndSessionEvent ();
//                }
//                ResetRealSessionTime();
//
//                SendStartSesionEvent ();
//                coldSessionStarted = true;
//            }
//        }
//    }

    // Real session time

    private static float GetRealSessionTime()
    {
        return PlayerPrefs.GetFloat("RealSessionTime", 0f);
    }

    private static void SetRealSessionTime(float time)
    {
        PlayerPrefs.SetFloat("RealSessionTime", time);
    }

    private static void ResetRealSessionTime()
    {
        SetRealSessionTime(0f);
    }


    //---------------------------
    // SERVICE
    //---------------------------

    private static int GetBalance()
    {
//        int _value = PrefsManager.CoinsCount.GetValue();
//        _value -= _value % 5;
        int _value = 0;
        return Mathf.Clamp(_value, 0, 1495);
    }

    private static int GetTimeTotalInMin()
    {
        var time = AppSeconds.GetSeconds();
        time = Mathf.FloorToInt(time / 60f);
        time = Mathf.Clamp(time, 0, 299);
        return time;
    }

    private static int GetScore()
    {
        var _value = PrefsManager.CurrentPointsCount;
        _value -= _value % 2;
        return Mathf.Clamp(_value, 0, 600);
    }

    private static int TimeToSessionTime(float time)
    {
        var sessionTime = Mathf.RoundToInt(time / 15f) * 15;
        sessionTime = Mathf.Clamp(sessionTime, 15, 4500);
        return sessionTime;
    }

    //---------------------------
    // EVENTS
    //---------------------------

    public static void SendEvent(string _eventName, string _origin = null, bool _isBalance = true, int _balanceAdd = 0)
    {
        /*FlurryEvent flurryEvent = new FlurryEvent(_eventName);
        if (_isBalance) {
            flurryEvent.AddParameter ("strawberries_balance", GetBalance () + _balanceAdd);
        }
        flurryEvent.AddParameter("game_time", GetTimeTotalInMin());
        if (_origin != null) flurryEvent.AddParameter("origin", _origin);

        FlurryEvents.LogEvent (flurryEvent);*/
    }

    public static void SendEventPlayed(bool revive, string fail_reason)
    {
        /*FlurryEvent flurryEvent = new FlurryEvent("played");
        flurryEvent.AddParameter("strawberries_balance", GetBalance());
        flurryEvent.AddParameter("game_time", GetTimeTotalInMin());
        flurryEvent.AddParameter("score", GetScore());
        flurryEvent.AddParameter("revive", revive);
        flurryEvent.AddParameter("fail_reason", fail_reason);

        FlurryEvents.LogEvent (flurryEvent);*/
    }

    public static void SendStartEvent(string _eventName)
    {
        /*
        if (_eventName == "start_screen_length") startScreenLengthOpened = true; else
            if (_eventName == "attempt_length") attemptLength = true; else
                if (_eventName == "iap_shop_length") iapShopLengthOpened = true; else
                    if (_eventName == "candy_shop_length") candyShopOpened = true;


        FlurryEvent flurryEvent = new FlurryEvent(_eventName, true);
        FlurryEvents.LogEvent (flurryEvent);
        */
    }

    public static void SendEndEvent(string _eventName, bool _onAppPaused = false)
    {
        /*if (!_onAppPaused) {
            if (_eventName == "start_screen_length")
                startScreenLengthOpened = false;
            else if (_eventName == "attempt_length")
                attemptLength = false;
            else if (_eventName == "iap_shop_length")
                iapShopLengthOpened = false;
            else if (_eventName == "candy_shop_length")
                candyShopOpened = false;
        }

        FlurryEndEvent flurryEndEvent = new FlurryEndEvent (_eventName);
        FlurryEvents.EndLogEvent (flurryEndEvent);*/
    }

    private static void SendStartSesionEvent()
    {
        /*FlurryEvent flurryEvent = new FlurryEvent("session_start");
        flurryEvent.AddParameter("strawberries_balance", GetBalance());

        FlurryEvents.LogEvent (flurryEvent);*/
    }

    public static void SendEndSessionEvent()
    {
        /*FlurryEvent flurryEvent = new FlurryEvent("session_end");
        flurryEvent.AddParameter("strawberries_balance", GetBalance());
        flurryEvent.AddParameter("session_time", TimeToSessionTime (GetRealSessionTime()));

        FlurryEvents.LogEvent (flurryEvent);

        ResetRealSessionTime ();*/
    }
}