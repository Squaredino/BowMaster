using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialog : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Transform _window;
	
	[SerializeField] private Image _icon;
	[SerializeField] private Text _description;
	[SerializeField] private Text _progressText;
	[SerializeField] private Image _slider;
	
	private void Start()
	{
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
		_image.transform.localScale = new Vector2(0f, transform.localScale.y);
		_window.localScale = new Vector2(0f, transform.localScale.y);
	}

	private void OnEnable()
	{
		ScreenSkinsArrowsBtn.OnShowQuestDialog += OnShowQuestDialogArrow;
		ScreenSkinsTargetsBtn.OnShowQuestDialog += OnShowQuestDialogTarget;
		GlobalEvents<OnSendQuest>.Happened += OnSendQuest;
	}

	private void OnSendQuest(OnSendQuest obj)
	{
		_image.DOColor(new Color(_image.color.r, _image.color.g, _image.color.b, 0.8f), 0.2f);
		_image.transform.localScale = new Vector2(1f, 1f);
		_window.DOScaleX(1f, 0.3f).SetEase(Ease.InOutBack);

		if (obj.QuestItem.skinType == SkinType.Arrow)
		{
			_icon.sprite = Resources.Load<Sprite>("Gfx/Arrows/arrow_" + obj.QuestItem.skinId);
			_icon.transform.DOLocalRotate(new Vector3(0f, 0f, -45f), 0.2f).SetEase(Ease.InOutBack);
		}
		else {
			_icon.sprite = Resources.Load<Sprite>("Gfx/TargetsFull/target_" + obj.QuestItem.skinId);
			_icon.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.0f);
		}
	_icon.SetNativeSize();

		_description.text = obj.QuestItem.description;
		_progressText.text = obj.QuestItem.progress + "/" + obj.QuestItem.total;
		_slider.fillAmount = obj.QuestItem.progress / obj.QuestItem.total;
	}

	private void OnShowQuestDialogArrow(int id)
	{
		GlobalEvents<OnGetQuest>.Call(new OnGetQuest{Type = 0, Id = id});
	}
	
	private void OnShowQuestDialogTarget(int id)
	{
		GlobalEvents<OnGetQuest>.Call(new OnGetQuest{Type = 1, Id = id});
	}

	public void Close()
	{
		_image.DOColor(new Color(_image.color.r, _image.color.g, _image.color.b, 0f), 0.2f);
		_image.transform.localScale = new Vector2(0f, transform.localScale.y);
		_window.DOScaleX(0f, 0.2f);
	}
}
