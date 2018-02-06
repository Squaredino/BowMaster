using System;
using System.Collections.Generic;
using DG.Tweening;
using PrefsEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenSkinsTargetsBtn : ScreenSkinsBtn {
	public static event Action <int> OnShowQuestDialog;

	private void Start()
	{
//		SecurePlayerPrefs.SetInt("targetsAvailable_" + _id, 1);
		if (_id == 0) SetLock(false); else
		if (SecurePlayerPrefs.GetInt("targetsAvailable_" + _id) != 1)
		{
			SetLock(true);
		}
		transform.localScale = Vector3.zero;
	}

	private void OnEnable()
	{
		GlobalEvents<OnBtnTargetsShow>.Happened += OnBtnTargetsShow;
		GlobalEvents<OnBtnTargetsHide>.Happened += OnBtnTargetsHide;
		GlobalEvents<OnOpenSkinTarget>.Happened += OnOpenSkinTarget;
	}

	private void OnOpenSkinTarget(OnOpenSkinTarget obj)
	{
		if (obj.Id == _id) OpenSkin();
	}

	private void OnBtnTargetsShow(OnBtnTargetsShow obj)
	{
		transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutBack);
	}
	
	private void OnBtnTargetsHide(OnBtnTargetsHide obj)
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
		if (SecurePlayerPrefs.GetInt("targetsAvailable_" + _id) == 1 || _id == 0)
		{
			ScreenSkins.CurrentTargetId = _id;	
			SecurePlayerPrefs.SetInt("CurrentTargetId", ScreenSkins.CurrentTargetId);
			SetLock(false);
			GlobalEvents<OnChangeTargetSkin>.Call(new OnChangeTargetSkin{Id = _id});
//			MasterAudio.PlaySoundAndForget("GUI_Grab");
			GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
		} else
		{
			GameEvents.Send(OnShowQuestDialog, _id+1);
		}
	}
	
	protected void OpenSkin()
	{
		SecurePlayerPrefs.SetInt("targetsAvailable_" + _id, 1);
		SetLock(false);
		++PrefsManager.QuestCharactersCounter;
		SecurePlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", PrefsManager.QuestCharactersCounter);
	}
}
