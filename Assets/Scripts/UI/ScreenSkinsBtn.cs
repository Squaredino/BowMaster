using UnityEngine;
using UnityEngine.UI;

public class ScreenSkinsBtn : MonoBehaviour
{
	[SerializeField] protected int _id;
	[SerializeField] protected Image _buttonImage;

	public virtual void SetLock(bool isUnlocked)
	{
		if (isUnlocked)
		{
			_buttonImage.color = new Color(.25f, .25f, .25f, 1);
		}
		else
		{
			_buttonImage.color = new Color(1, 1, 1, 1);
		}
	}
	
	// Выбираем скин, который хотим установить
	public virtual void Click()
	{
		Debug.Log("Click " + _id);
	}
}
