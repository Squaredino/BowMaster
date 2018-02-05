using DG.Tweening;
using UnityEngine;

public class ScreenMenuBtnSkins : MonoBehaviour
{
	[SerializeField] private GameObject _sign;

	private void Awake()
	{
		_sign.SetActive(false);
	}

	private void OnEnable()
	{
		GlobalEvents<OnOpenSkinArrow>.Happened += OnOpenSkinArrow;
		GlobalEvents<OnOpenSkinTarget>.Happened += OnOpenSkinTarget;
		GlobalEvents<OnScreenSkinsHide>.Happened += OnScreenSkinsHide;
	}

	private void OnScreenSkinsHide(OnScreenSkinsHide obj)
	{
		_sign.SetActive(false);
	}

	private void OnOpenSkinArrow(OnOpenSkinArrow obj)
	{
		if (obj.Id > 2) ShowSign();
	}

	private void OnOpenSkinTarget(OnOpenSkinTarget obj)
	{
		if (obj.Id > 2) ShowSign();
	}

	private void ShowSign()
	{
		_sign.SetActive(true);
		_sign.transform.DOLocalRotate(new Vector3(0f, 0f, 15f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
	}

	public void Click()
	{
		GlobalEvents<OnScreenSkinsShow>.Call(new OnScreenSkinsShow());
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable {Flag = false});
	}
}
