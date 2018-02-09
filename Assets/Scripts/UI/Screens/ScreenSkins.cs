using System.Collections.Generic;
using DG.Tweening;
//using DarkTonic.MasterAudio;
using PrefsEditor;
using QuickEngine.Extensions;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenSkins : MonoBehaviour
{
    [SerializeField] private GameObject _staff;
    
    public const int CHARACTER_REWARDED_1 = 23; 
    
    public static int CurrentFaceId;
    public static int CurrentTargetId;

    private RectTransform _rt;
    private bool _isSkinsAllGeneralOpened;

    private void Awake()
    {
        CurrentFaceId = SecurePlayerPrefs.GetInt("currentFaceID");
        CurrentTargetId = SecurePlayerPrefs.GetInt("CurrentTargetId");
    }

    private void Start()
    {
        _rt = GetComponent<RectTransform>();
        transform.localPosition = new Vector2(-_rt.GetWidth(), 0f);
        AreThereSkins();
        AreThereSkinsGeneral();

        AddEvents();
        _staff.SetActive(false);
    }

    private void AddEvents()
    {
        GlobalEvents<OnBuySkinByIAP>.Happened += OnBuySkinByIAP;
        GlobalEvents<OnBuySkinByRewarded>.Happened += OnBuySkinByRewarded;
        GlobalEvents<OnSkinsUnlockAll>.Happened += OnSkinsUnlockAll;
        GlobalEvents<OnScreenSkinsShow>.Happened += OnScreenSkinsShow;
        GlobalEvents<OnScreenSkinsHide>.Happened += OnScreenSkinsHide;
    }

    private void OnScreenSkinsShow(OnScreenSkinsShow obj)
    {
        Show();
    }

    private void OnScreenSkinsHide(OnScreenSkinsHide obj)
    {
        Hide();
    }
    
    private void Show()
    {
        _staff.SetActive(true);
        transform.DOLocalMoveX(0f, 0.2f);
    }
    
    private void Hide()
    {
        transform.DOLocalMoveX(-_rt.GetWidth(), 0.2f).OnComplete(() =>
        {
            _staff.SetActive(false);
        });
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable {Flag = true});
    }
    
//    // Выбираем скин, который хотим установить
//    public void SetSkin(int id)
//    {
//        Analytics.CustomEvent("SkinsSkinClick",
//            new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter},{"id", id}});
//        bool isAvailable = false;
//        if (_faceAll[id] == 1)
//        {
//            CurrentFaceId = id;
//            GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = id});
//            SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
//            ChooseColorForButtons();
//            isAvailable = true;
//        } else
//        if (id == CHARACTER_REWARDED_1)
//        {
//            GlobalEvents<OnAdsRewardedBuySkin>.Call(new OnAdsRewardedBuySkin{Id = id});
//            OpenSkin(id);
//            isAvailable = true;
//        } 
//        // это код покупки скина
////        else 
////            if (PrefsManager.CoinsCount.GetValue() >= 200 /*PrefsManager.FacePrice[_id - 1]*/)
////            {
////                BuySkin(id);
////                isAvailable = true;
////            }
//
//        if (isAvailable)
//        {
////            MasterAudio.PlaySoundAndForget("GUI_Grab");
//            Hide();
//            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
//        } 
//    }
    
    // Нажали на платный скин
    public void BuyPayableSkin(int id)
    {
//        int realId = id + IapSkin1 - 1;
//        if (_faceAll[realId] == 1)
//        {
//            CurrentFaceId = realId;
//            GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = realId});
//            SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
//            ChooseColorForButtons();
////            MasterAudio.PlaySoundAndForget("GUI_Grab");
//            Hide();
//            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
//        }
//        else
//        {
//            if (id == 1) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin1});
//            else if (id == 2)
//                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin2});
//            else if (id == 3)
//                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin3});
//            else if (id == 4)
//                GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin {Id = BillingManager.iapTierSkin4});
//        }
    }

    // Открыть скин за просмотр рекламы
    private void OnBuySkinByRewarded(OnBuySkinByRewarded obj)
    {
//        switch (obj.Id)
//        {
//            case CHARACTER_REWARDED_1: OpenSkin(CHARACTER_REWARDED_1);
//                break;
//        }
//        MasterAudio.PlaySoundAndForget("GUI_Grab");
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }

    // Открыть все скины
    private void OnSkinsUnlockAll(OnSkinsUnlockAll obj)
    {
//        for (var i = 0; i < _faceAll.Length; i++)
//        {
//            SecurePlayerPrefs.SetInt("faceAvailable_" + i, 1);
//            _faceAll[i] = 1;
//        }
//
//        PrefsManager.QuestCharactersCounter = _faceAll.Length;
//        
//        ChooseColorForButtons();
//        AreThereSkins();
//        AreThereSkinsGeneral();
    }
    
    // Пришел Ивент о покупки скина
    private void OnBuySkinByIAP(OnBuySkinByIAP obj)
    {
//        switch (obj.Id)
//        {
//            case BillingManager.iapTierSkin1: OpenSkin(IapSkin1);
//                break;
//            case BillingManager.iapTierSkin2: OpenSkin(IapSkin2);
//                break;
//            case BillingManager.iapTierSkin3: OpenSkin(IapSkin3);
//                break;
//            case BillingManager.iapTierSkin4: OpenSkin(IapSkin4);
//                break;
//        }
//        MasterAudio.PlaySoundAndForget("GUI_Grab");
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }

    private void BuySkin(int id)
    {
        Analytics.CustomEvent("SkinsSkinBuy",
            new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter},{"id", id}});
//        OpenSkin(id);

        GlobalEvents<OnGotNewCharacter>.Call(new OnGotNewCharacter());
    }

    // Меняем цвет иконки скина, в зависимости от состояния (открыта/закрыта)
    private void ChooseColorForButtons()
    {
//        for (var i = 0; i < _faceAll.Length; i++)
//            if (_faceAll[i] == 1)
//            {
//                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(true);
//            }
//            else
//            {
//                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(false);
//            }
    }
    
    // Проверяем есть ли не открытые скины
    private void AreThereSkins()
    {
//        for (var i = 1; i < _faceAll.Length; i++)
//            if (_faceAll[i] == 0)
//            {
//                return;
//            }
//        GlobalEvents<OnSkinAllOpened>.Call(new OnSkinAllOpened());
    }
    
    // Проверяем, если ли не открытые скины в разделе General
    private void AreThereSkinsGeneral()
    {
//        for (var i = FacesGeneralMin; i <= FacesGeneralMax; i++)
//            if (_faceAll[i] == 0)
//            {
//                return;
//            }
//        _isSkinsAllGeneralOpened = true;
//        GlobalEvents<OnSkinAllGeneralOpened>.Call(new OnSkinAllGeneralOpened());
    }
}