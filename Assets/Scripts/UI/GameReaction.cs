using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameReaction : MonoBehaviour
{
	[SerializeField] private Image _background;
	[SerializeField] private Color bgHitColor, bgPerfectColor, bgGameOverColor;
	private int _perectCounter;
	private const float cameraShakeDuration = 0.2f, cameraShakeStrength = 0.1f;
	private const int cameraShakeVibratio = 30;
	public const float bgColorInDuration = 0.05f, bgColorOutDuration = 0.8f;

	private void Start()
	{
		_background.color = Color.white;
	}

	private void OnEnable()
	{
		GlobalEvents<OnStartGame>.Happened += OnStartGame;
		GlobalEvents<OnGameOver>.Happened += OnGameOver;
		GlobalEvents<OnIventSimple>.Happened += OnIventSimple;
		GlobalEvents<OnIventPerfect>.Happened += OnIventPerfect;
	}

	private void OnStartGame(OnStartGame obj)
	{
		_perectCounter = 0;
	}
	
	private void OnIventSimple(OnIventSimple obj)
	{
		_perectCounter = 0;
	}

	private void OnIventPerfect(OnIventPerfect obj)
	{
		++_perectCounter;
		if (_perectCounter >= 3)
		{
			GlobalEvents<OnVibrate>.Call(new OnVibrate());
			Camera.main.DOShakePosition(cameraShakeDuration, cameraShakeStrength, cameraShakeVibratio);
		}
		_background.DOKill();
		_background.DOColor(_perectCounter > 2 ? bgPerfectColor : bgHitColor, bgColorInDuration)
			.OnComplete(() => _background.DOColor(Color.white, bgColorOutDuration));
	}

	private void OnGameOver(OnGameOver obj)
	{
		_perectCounter = 0;
		Camera.main.DOShakePosition(cameraShakeDuration, 0.2f, cameraShakeVibratio);
		GlobalEvents<OnVibrate>.Call(new OnVibrate());
		_background.DOColor(bgGameOverColor, 0.5f).SetEase(Ease.OutQuart)
			.OnComplete(() => _background.DOColor(Color.white, 0.5f).SetEase(Ease.InQuart));
	}
}
