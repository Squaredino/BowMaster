using DG.Tweening;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestUnlockedDialog : MonoBehaviour
{
	[SerializeField] private Image _background;
	[SerializeField] private Transform _window;
	[SerializeField] private Image _icon;

	private Quest _quest;
	
	private void Start()
	{
		transform.localPosition = Vector2.zero;
		_background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0f);
		_background.transform.localScale = new Vector2(0f, transform.localScale.y);
		_window.localScale = new Vector2(0f, transform.localScale.y);
	}

	private void OnEnable()
	{
		GlobalEvents<OnQuestShowUnlockedDialog>.Happened += OnQuestShowUnlockedDialog;
		GlobalEvents<OnScreenSkinsHide>.Happened += OnScreenSkinsHide;
	}

	private void OnQuestShowUnlockedDialog(OnQuestShowUnlockedDialog obj)
	{
		_quest = obj.QuestItem;
		_background.DOColor(new Color(_background.color.r, _background.color.g, _background.color.b, 0.8f), 0.2f);
		_background.transform.localScale = new Vector2(1f, 1f);
		_window.DOScaleX(1f, 0.3f).SetEase(Ease.InOutBack);

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
		_quest = null;
	}

	public void Click()
	{
		ScreenSkins.CurrentFaceId = _quest.skinId;	
		SecurePlayerPrefs.SetInt("currentFaceID", ScreenSkins.CurrentFaceId);
		GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = ScreenSkins.CurrentFaceId});
		Close();
	}
}
