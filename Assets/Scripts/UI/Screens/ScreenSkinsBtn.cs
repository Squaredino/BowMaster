using UnityEngine;
using UnityEngine.UI;

public class ScreenSkinsBtn : MonoBehaviour {

	private Image _buttonImage;

	private void Start()
	{
		_buttonImage = GetComponentInChildren<Image>();
	}

	public void SetLock(bool isUnlocked)
	{
		if (isUnlocked)
		{
			_buttonImage.color = new Color(1,1,1,1);
		}
		else
		{
			_buttonImage.color = new Color(0,0,0,1);
		}
	}
}
