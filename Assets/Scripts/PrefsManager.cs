using System;
using PrefsEditor;
using UnityEngine;

public class PrefsManager : MonoBehaviour
{
    public const int GameVersion = 0;

    public static int GameplayCounter = 0; // Считает количество игр сыгранных в этой игровой сессии
    public static int CurrentPointsCount = 0;
    public static int GameBestScore; // Лучший счет
    
    public static bool IsFirstBuy; // Первая покупка была произведена 

    public static int QuestGameplayCounter;
    public static int QuestThrowCounter;
    public static int QuestCharactersCounter;
    public static int QuestBombsCounter;
    public static int QuestMissCounter;
    
    private void Awake()
    {
        SecurePlayerPrefs.PassPhrase = "squaredino.com";
        SecurePlayerPrefs.UseSecurePrefs = true;
        OnLoadStatistics();
    }
    
    private void OnLoadStatistics()
    {
        IsFirstBuy = SecurePlayerPrefs.GetBool("IsFirstBuy");
        GameBestScore = SecurePlayerPrefs.GetInt("BestScore");
//      gameBestScore = 0;
        
        if (PlayerPrefs.GetInt("GameVersion", -1) < GameVersion)
        {
            PlayerPrefs.SetInt("GameVersion", GameVersion);
            PlayerPrefs.SetInt("RateCounter", 0);
        }

        QuestGameplayCounter = PlayerPrefs.GetInt("QUEST_GAMEPLAY_Counter", 0);
        // TEMP
//        QuestGameplayCounter = 0;
        
        QuestThrowCounter = PlayerPrefs.GetInt("QUEST_THROW_Counter", 0);
        QuestBombsCounter = PlayerPrefs.GetInt("QUEST_BOMBS_Counter", 0);
        QuestMissCounter = PlayerPrefs.GetInt("QUEST_MISS_Counter", 0);
    }

    // Music Enabled
//    
//    public static bool LoadMusicEnabled()
//    {
//        return SecurePlayerPrefs.GetBool("MusicEnabled", true);
//    }
//
//    public static void SaveMusicEnabled(bool value)
//    {
//        SecurePlayerPrefs.SetBool("MusicEnabled", value);
//        SecurePlayerPrefs.Save();
//    }

    // Sound Enabled
    
//    public static bool LoadSoundEnabled()
//    {
//        return SecurePlayerPrefs.GetBool("SoundEnabled", true);
//    }
//
//    public static void SaveSoundEnabled(bool value)
//    {
//        SecurePlayerPrefs.SetBool("SoundEnabled", value);
//        SecurePlayerPrefs.Save();
//    }
    
    // Video Enabled
    
//    public static bool LoadVideoEnabled()
//    {
//        return SecurePlayerPrefs.GetBool("VideoEnabled", true);
//    }
//
//    public static void SaveVideoEnabled(bool value)
//    {
//        SecurePlayerPrefs.SetBool("VideoEnabled", value);
//        SecurePlayerPrefs.Save();
//    }
    
    // Open Level Index

//    public static int LoadOpenLevelIndex()
//    {
//        return SecurePlayerPrefs.GetInt("OpenLevelIndex", 1);
//    }
//
//    public static void SaveOpenLevelIndex(int value)
//    {      
//        SecurePlayerPrefs.SetInt("OpenLevelIndex", value);
//        SecurePlayerPrefs.Save();
//    }
    
    // Max Level Passed
//
//    public static bool LoadMaxLevelPassed()
//    {
//        return SecurePlayerPrefs.GetBool("MaxLevelPassed", false);
//    }
//
//    public static void SaveMaxLevelPassed(bool value)
//    {
//        SecurePlayerPrefs.SetBool("MaxLevelPassed", value);
//    }
    
    // Ads Enabled

//    public static bool LoadAdsEnabled()
//    {
//        return SecurePlayerPrefs.GetBool("AdsEnabled", true);
//    }
//
//    public static void SaveAdsEnabled(bool value)
//    {
//        SecurePlayerPrefs.SetBool("AdsEnabled", value);
//        SecurePlayerPrefs.Save();
//    }
    
    // Ad Lock Enabled

//    public static bool LoadAdLockEnabled()
//    {
//        return SecurePlayerPrefs.GetBool("AdLockEnabled", true);
//    }
//
//    public static void SaveAdLockEnabled(bool value)
//    {
//        SecurePlayerPrefs.SetBool("AdLockEnabled", value);
//        SecurePlayerPrefs.Save();
//    }
    
    // Timestamp

    public static DateTime LoadTimestamp(String id)
    {
        long timestamp = Convert.ToInt64(SecurePlayerPrefs.GetString("Timestamp_" + id, "0"));
        return timestamp == 0 ? UnbiasedTime.Instance.Now() : DateTime.FromBinary(timestamp);
    }

    public static void SaveTimestamp(String id, DateTime value)
    {
        SecurePlayerPrefs.SetString("Timestamp_" + id, value.ToBinary().ToString());
    }
    
    // Timer Enabled

    public static bool LoadTimerEnabled(String id)
    {
        return SecurePlayerPrefs.GetBool("TimerEnabled_" + id);
    }

    public static void SaveTimerEnabled(String id, bool value)
    {
        SecurePlayerPrefs.SetBool("TimerEnabled_" + id, value);
        SecurePlayerPrefs.Save();
    }
}
