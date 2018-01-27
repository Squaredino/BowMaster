using DG.Tweening;
using TMPro;
using UnityEngine;

public class PointsBalloon : MonoBehaviour
{
	private TextMeshProUGUI _text;
	private const float ShowTime = 0.7f;

	public void SetCount(int value)
	{
		_text = GetComponent<TextMeshProUGUI>();
		Destroy(gameObject, ShowTime);
		
		if (value == 0) return;
		
		float y = transform.localPosition.y + 53f;
		transform.DOLocalMoveY(y, ShowTime);
		
		
		_text.text = "+" + value;
		if (value == 1)
		{
			_text.color = Color.black;
			_text.DOColor(new Color(0f, 0f, 0f, 0f), ShowTime);
		}
		else
		{
			_text.color = Color.white;
			_text.DOColor(new Color(1f, 1f, 1f, 0f), ShowTime);
		}

		if (value >= 3)
		{
			transform.Rotate(Vector3.forward, -11 + 11f/value);
			transform.DOScale(1.5f, ShowTime*2f/value).SetLoops(-1, LoopType.Yoyo);
			transform.DOLocalRotate(new Vector3(0f, 0f, 11f - 11f/value), ShowTime/value).SetLoops(-1, LoopType.Yoyo);
		}
	}
}
