﻿using System.Collections.Generic;
//using DarkTonic.MasterAudio;
using DoozyUI;
using PrefsEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenSkins : ScreenItem
{
    [SerializeField] private GameObject[] _skinBtns;
    [SerializeField] private GameObject _choosedSkin;
    
    public const int CHARACTER_REWARDED_1 = 23; 
    
    public static int CurrentFaceId;
    public static int CurrentTargetId;
    private const int FacesGeneralMin = 0;
    private const int FacesGeneralMax = 5;
    private const int FacesSocialStartId = FacesGeneralMax+1;
    private const int FacesPaybleStartId = FacesSocialStartId + 4;

    private const int SkinFacebook = FacesSocialStartId;
    private const int SkinTwitter = FacesSocialStartId + 1;
    private const int SkinInstagram = FacesSocialStartId + 2;
    private const int SkinSponsor = FacesSocialStartId + 3;
    
    private const int IapSkin1 = FacesPaybleStartId;
    private const int IapSkin2 = FacesPaybleStartId + 1;
    private const int IapSkin3 = FacesPaybleStartId + 2;
    private const int IapSkin4 = FacesPaybleStartId + 3;
    private int[] _faceAll = {
        // General
        1, 0, 0, 0, 0, 0,
        // Social
        0, 0, 0, 0,
        // Money
        0, 0, 0, 0};
    
    private bool _isSkinsAllGeneralOpened;

    private void Awake()
    {
        CurrentFaceId = SecurePlayerPrefs.GetInt("currentFaceID");
        CurrentTargetId = SecurePlayerPrefs.GetInt("CurrentTargetId");
        CurrentFaceId = 0;
        CurrentTargetId = 1;
        for (var i = 0; i < _faceAll.Length; i++)
            SecurePlayerPrefs.SetInt("faceAvailable_" + i, 1);
        
        for (var i = 1; i < _faceAll.Length; i++)
            _faceAll[i] = SecurePlayerPrefs.GetInt("faceAvailable_" + i);

        for (var i = 0; i < _faceAll.Length; i++)
        {
            if (_faceAll[i] == 1) ++PrefsManager.QuestCharactersCounter;
        }
    }

    private void Start()
    {
        InitUi();
        AreThereSkins();
        AreThereSkinsGeneral();

        AddEvents();
    }

    private void AddEvents()
    {
        GlobalEvents<OnBuySkinByIAP>.Happened += OnBuySkinByIAP;
        GlobalEvents<OnBuySkinByRewarded>.Happened += OnBuySkinByRewarded;
        GlobalEvents<OnSkinsUnlockAll>.Happened += OnSkinsUnlockAll;
    }
    
    public override void Show()
    {
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
        
        base.Show();
        
        Invoke("ChooseColorForButtons", 1f);
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
    }

    public override void Hide()
    {
        base.Hide();
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = true});
    }
    
    // Выбираем скин, который хотим установить
    public void SetSkin(int id)
    {
        Analytics.CustomEvent("SkinsSkinClick",
            new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter},{"id", id}});
        bool isAvailable = false;
        if (_faceAll[id] == 1)
        {
            CurrentFaceId = id;
            GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = id});
            SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
            ChooseColorForButtons();
            isAvailable = true;
        } else
        // Social
        if (id == SkinFacebook)
        {
            Application.OpenURL("https://www.facebook.com/Squaredino/");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinTwitter)
        {
            Application.OpenURL("http://twitter.com/Soulghai");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinInstagram)
        {
            Application.OpenURL("https://www.instagram.com/squaredino/");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinSponsor)
        {
            Application.OpenURL("http://www.squaredino.com");
            OpenSkin(id);
            isAvailable = true;
        } 
        else 
        if (id == CHARACTER_REWARDED_1)
        {
            GlobalEvents<OnAdsRewardedBuySkin>.Call(new OnAdsRewardedBuySkin{Id = id});
            OpenSkin(id);
            isAvailable = true;
        } 
        // это код покупки скина
//        else 
//            if (PrefsManager.CoinsCount.GetValue() >= 200 /*PrefsManager.FacePrice[_id - 1]*/)
//            {
//                BuySkin(id);
//                isAvailable = true;
//            }

        if (isAvailable)
        {
//            MasterAudio.PlaySoundAndForget("GUI_Grab");
            Hide();
            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
        } 
    }
    
    // Нажали на платный скин
    public void BuyPayableSkin(int id)
    {
        int realId = id + IapSkin1 - 1;
        if (_faceAll[realId] == 1)
        {
            CurrentFaceId = realId;
            GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = realId});
            SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
            ChooseColorForButtons();
//            MasterAudio.PlaySoundAndForget("GUI_Grab");
            Hide();
            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
        }
        else
        {
            if (id == 1) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin1});
            else if (id == 2)
                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin2});
            else if (id == 3)
                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin3});
            else if (id == 4)
                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin4});
        }
    }
    
    // Открыть скин за просмотр рекламы
    private void OnBuySkinByRewarded(OnBuySkinByRewarded obj)
    {
        switch (obj.Id)
        {
            case CHARACTER_REWARDED_1: OpenSkin(CHARACTER_REWARDED_1);
                break;
        }
//        MasterAudio.PlaySoundAndForget("GUI_Grab");
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }

    // Открыть все скины
    private void OnSkinsUnlockAll(OnSkinsUnlockAll obj)
    {
        for (var i = 0; i < _faceAll.Length; i++)
        {
            SecurePlayerPrefs.SetInt("faceAvailable_" + i, 1);
            _faceAll[i] = 1;
        }

        PrefsManager.QuestCharactersCounter = _faceAll.Length;
        
        ChooseColorForButtons();
        AreThereSkins();
        AreThereSkinsGeneral();
    }
    
    // Пришел Ивент о покупки скина
    private void OnBuySkinByIAP(OnBuySkinByIAP obj)
    {
        switch (obj.Id)
        {
            case BillingManager.iapTierSkin1: OpenSkin(IapSkin1);
                break;
            case BillingManager.iapTierSkin2: OpenSkin(IapSkin2);
                break;
            case BillingManager.iapTierSkin3: OpenSkin(IapSkin3);
                break;
            case BillingManager.iapTierSkin4: OpenSkin(IapSkin4);
                break;
        }
//        MasterAudio.PlaySoundAndForget("GUI_Grab");
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }

    private void BuySkin(int id)
    {
        Analytics.CustomEvent("SkinsSkinBuy",
            new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter},{"id", id}});
        OpenSkin(id);

//        GlobalEvents<AchievementProgress>.Call(new AchievementProgress
//        {
//            Id = GameServicesManager.ACHIEVEMENT_NEW_SKIN, Progress = 1
//        });
//        GlobalEvents<AchievementProgress>.Call(new AchievementProgress
//        {
//            Id = GameServicesManager.ACHIEVEMENT_COLLECTION, Progress = PrefsManager.QuestCharactersCounter
//        });
        GlobalEvents<OnGotNewCharacter>.Call(new OnGotNewCharacter());
    }

    private void OpenSkin(int id)
    {
        _faceAll[id] = 1;
        CurrentFaceId = id;
        SecurePlayerPrefs.SetInt("currentFaceID", id);
        SecurePlayerPrefs.SetInt("faceAvailable_" + id, 1);
        ++PrefsManager.QuestCharactersCounter;
        SecurePlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", PrefsManager.QuestCharactersCounter);
        ChooseColorForButtons();
        AreThereSkins();
        AreThereSkinsGeneral();
        GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = id});
    }

    // Меняем цвет иконки скина, в зависимости от состояния (открыта/закрыта)
    private void ChooseColorForButtons()
    {
        for (var i = 0; i < _faceAll.Length; i++)
            if (_faceAll[i] == 1)
            {
                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(true);
            }
            else
            {
                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(false);
            }
        
        _choosedSkin.transform.position = _skinBtns[CurrentFaceId].transform.position;
    }
    
    // Проверяем есть ли не открытые скины
    private void AreThereSkins()
    {
        for (var i = 1; i < _faceAll.Length; i++)
            if (_faceAll[i] == 0)
            {
                return;
            }
        GlobalEvents<OnSkinAllOpened>.Call(new OnSkinAllOpened());
    }
    
    // Проверяем, если ли не открытые скины в разделе General
    private void AreThereSkinsGeneral()
    {
        for (var i = FacesGeneralMin; i <= FacesGeneralMax; i++)
            if (_faceAll[i] == 0)
            {
                return;
            }
        _isSkinsAllGeneralOpened = true;
        GlobalEvents<OnSkinAllGeneralOpened>.Call(new OnSkinAllGeneralOpened());
    }
}