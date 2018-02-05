using DG.Tweening;
using UnityEngine;

public class ScreenMenu : MonoBehaviour
{
	[SerializeField] private GameObject _btnSkins;
	[SerializeField] private GameObject _btnVibro;

	private float _BtnSkinInitPlace;
	private float _BtnVibroInitPlace;
	
	private const float _leftBtnPlace = 400f;
	private const float _rightBtnPlace = 400f;

	private void Start()
	{
		_BtnSkinInitPlace = _btnSkins.transform.position.x;
		_BtnVibroInitPlace = _btnVibro.transform.position.x;
	}

	private void OnEnable()
	{
		GlobalEvents<OnStartGame>.Happened += OnStartGame;
		GlobalEvents<OnGameOver>.Happened += OnGameOver;
	}

	private void OnStartGame(OnStartGame obj)
	{
		_btnSkins.transform.DOMoveX(_btnSkins.transform.position.x - _leftBtnPlace, 0.3f);
		_btnVibro.transform.DOMoveX(_btnVibro.transform.position.x + _rightBtnPlace, 0.3f);
	}
	
	private void OnGameOver(OnGameOver obj)
	{
		_btnSkins.transform.DOMoveX(_BtnSkinInitPlace, 0.3f);
		_btnVibro.transform.DOMoveX(_BtnVibroInitPlace, 0.3f);
	}
}
