using System.Runtime.Serialization.Formatters;
using DG.Tweening;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialog : MonoBehaviour
{
	[SerializeField] private Image _background;
	[SerializeField] private Transform _window;
	[SerializeField] private GameObject _btnEquip;
	[SerializeField] private GameObject _btnTry;
	
	[SerializeField] private Image _icon;
	[SerializeField] private Text _LockText;
	[SerializeField] private Text _description;
	[SerializeField] private Text _progressText;
	[SerializeField] private Image _slider;
	
	private Quest _quest;
	private bool _isCurrentQuestUnlocked;
	
	private void Start()
	{
		transform.localPosition = Vector2.zero;
		_background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0f);
		_background.transform.localScale = new Vector2(0f, transform.localScale.y);
		_window.localScale = new Vector2(0f, transform.localScale.y);
	}

	private void OnEnable()
	{
		ScreenSkinsArrowsBtn.OnShowQuestDialog += OnShowQuestDialogArrow;
		ScreenSkinsTargetsBtn.OnShowQuestDialog += OnShowQuestDialogTarget;
		GlobalEvents<OnSendQuest>.Happened += OnSendQuest;
		GlobalEvents<OnScreenSkinsHide>.Happened += OnScreenSkinsHide;
		GlobalEvents<OnQuestShowUnlockedDialog>.Happened += OnQuestShowUnlockedDialog;
	}

	private void OnQuestShowUnlockedDialog(OnQuestShowUnlockedDialog obj)
	{
		_isCurrentQuestUnlocked = true;
		FillDialog(obj.QuestItem);
	}

	private void OnSendQuest(OnSendQuest obj)
	{
		_isCurrentQuestUnlocked = false;
		FillDialog(obj.QuestItem);
	}

	private void FillDialog(Quest quest)
	{
		_quest = quest;
		_background.DOColor(new Color(_background.color.r, _background.color.g, _background.color.b, 0.8f), 0.2f);
		_background.transform.localScale = new Vector2(1f, 1f);
		_window.DOScaleX(1f, 0.3f).SetEase(Ease.InOutBack);

		if (_isCurrentQuestUnlocked)
		{
			_LockText.text = "UNLOCKED!";
			_LockText.color = new Color(1f, 0.8f, 0.1f);
			_btnEquip.SetActive(true);
			_btnTry.SetActive(false);
		}
		else
		{
			_LockText.text = "UNLOCK WITH";
			_LockText.color = Color.white;
			_btnEquip.SetActive(false);
			_btnTry.SetActive(true);
		}

		if (_quest.skinType == SkinType.Arrow)
		{
			_icon.sprite = Resources.Load<Sprite>("Gfx/Arrows/arrow_" + _quest.skinId);
			_icon.transform.DOLocalRotate(new Vector3(0f, 0f, -45f), 0.2f).SetEase(Ease.InOutBack);
			_icon.transform.localScale = new Vector2(4f, 4f);
		}
		else {
			_icon.sprite = Resources.Load<Sprite>("Gfx/TargetsFull/target_" + _quest.skinId);
			_icon.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.0f);
			_icon.transform.localScale = new Vector2(2f, 2f);
		}

		_icon.SetNativeSize();
		_description.text = _quest.description;
		_progressText.text = _quest.progress + "/" + _quest.total;
		_slider.fillAmount = (float)Mathf.Min(_quest.progress,_quest.total) / _quest.total;
	}

	private void OnShowQuestDialogArrow(int id)
	{
		GlobalEvents<OnGetQuest>.Call(new OnGetQuest{SkinType = SkinType.Arrow, Id = id});
	}
	
	private void OnShowQuestDialogTarget(int id)
	{
		GlobalEvents<OnGetQuest>.Call(new OnGetQuest{SkinType = SkinType.Target, Id = id});
	}
	
	private void OnScreenSkinsHide(OnScreenSkinsHide obj)
	{
		Close();
	}

	public void Close()
	{
		_background.DOColor(new Color(_background.color.r, _background.color.g, _background.color.b, 0f), 0.2f);
		_background.transform.localScale = new Vector2(0f, transform.localScale.y);
		_window.DOScaleX(0f, 0.2f);
	}
	
	public void BtnEquip()
	{
		if (_quest.skinType == SkinType.Arrow)
		{
			ScreenSkins.CurrentFaceId = _quest.skinId;	
			SecurePlayerPrefs.SetInt("currentFaceID", ScreenSkins.CurrentFaceId);
			GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin {Id = ScreenSkins.CurrentFaceId});
		}
		else
		{
			ScreenSkins.CurrentTargetId = _quest.skinId;
			SecurePlayerPrefs.SetInt("currentFaceID", ScreenSkins.CurrentTargetId);
			GlobalEvents<OnChangeTargetSkin>.Call(new OnChangeTargetSkin {Id = ScreenSkins.CurrentTargetId});
		}

		Close();
	}
	
	public void BtnTryClick()
	{
		if (_quest.skinType == SkinType.Arrow)
		{
			GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = _quest.skinId});
		}
		else
		{
			GlobalEvents<OnChangeTargetSkin>.Call(new OnChangeTargetSkin {Id = _quest.skinId});
		}
		Close();
		GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
	}
}
