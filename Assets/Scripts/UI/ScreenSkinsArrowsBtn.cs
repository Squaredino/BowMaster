﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using PrefsEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenSkinsArrowsBtn : ScreenSkinsBtn {
	public static event Action <int> OnShowQuestDialog;

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
		GlobalEvents<OnOpenSkin>.Happened += OnOpenSkin;
	}

	private void OnOpenSkin(OnOpenSkin obj)
	{
		if (obj.QuestItem.skinType == SkinType.Arrow)
		if (obj.QuestItem.skinId == _id) OpenSkin();
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
		
		Select();
	}

	public override void Select()
	{
		base.Select();
		if (SecurePlayerPrefs.GetInt("faceAvailable_" + _id) == 1 || _id == 0)
		{
			ScreenSkins.CurrentFaceId = _id;	
			SecurePlayerPrefs.SetInt("currentFaceID", ScreenSkins.CurrentFaceId);
			SetLock(false);
			GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = _id});
//			MasterAudio.PlaySoundAndForget("GUI_Grab");
			GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
		} else
		{
			GameEvents.Send(OnShowQuestDialog, _id);
		}
	}

	protected void OpenSkin()
	{
		SecurePlayerPrefs.SetInt("faceAvailable_" + _id, 1);
		SetLock(false);
		++PrefsManager.QuestCharactersCounter;
		SecurePlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", PrefsManager.QuestCharactersCounter);
	}
}
