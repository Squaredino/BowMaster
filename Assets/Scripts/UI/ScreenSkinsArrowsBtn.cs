using System.Collections.Generic;
using DG.Tweening;
using PrefsEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenSkinsArrowsBtn : ScreenSkinsBtn {

	private void Start()
	{
//		SecurePlayerPrefs.SetInt("faceAvailable_" + _id, 1);
		if (_id == 0) SetLock(false); else
		if (SecurePlayerPrefs.GetInt("faceAvailable_" + _id) != 1)
		{
			SetLock(true);
		}
//		transform.localScale = Vector3.zero;
	}

	private void OnEnable()
	{
		GlobalEvents<OnBtnArrowsShow>.Happened += OnShowBtnArrows;
		GlobalEvents<OnBtnArrowsHide>.Happened += OnBtnArrowsHide;
		GlobalEvents<OnOpenSkinArrow>.Happened += OnOpenSkinArrow;
	}

	private void OnOpenSkinArrow(OnOpenSkinArrow obj)
	{
		if (obj.Id == _id) OpenSkin();
	}

	private void OnShowBtnArrows(OnBtnArrowsShow obj)
	{
		transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutBack);
	}
	
	private void OnBtnArrowsHide(OnBtnArrowsHide obj)
	{
		transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBack);
	}
	
	// Выбираем скин, который хотим установить
	override public void Click()
	{
		Debug.Log("Click " + _id);
		Analytics.CustomEvent("SkinsSkinClick",
			new Dictionary<string, object> {{"sessions", PrefsManager.GameplayCounter},{"id", _id}});
		
		if (SecurePlayerPrefs.GetInt("faceAvailable_" + _id) == 1 || _id == 0)
		{
			ScreenSkins.CurrentFaceId = _id;	
			SecurePlayerPrefs.SetInt("currentFaceID", ScreenSkins.CurrentFaceId);
			SetLock(false);
			GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = _id});
//			MasterAudio.PlaySoundAndForget("GUI_Grab");
			GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
		}
	}
	
	protected void OpenSkin()
	{
		ScreenSkins.CurrentFaceId = _id;
		SecurePlayerPrefs.SetInt("currentFaceID", _id);
		SecurePlayerPrefs.SetInt("faceAvailable_" + _id, 1);
		SetLock(false);
		++PrefsManager.QuestCharactersCounter;
		SecurePlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", PrefsManager.QuestCharactersCounter);
		GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = _id});
	}
}
