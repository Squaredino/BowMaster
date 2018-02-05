using PrefsEditor;
using UnityEngine;

public class ScreenSkinsArrowsBtnInstagram : ScreenSkinsArrowsBtn
{
	[SerializeField] private GameObject[] _objects;
	
	private void Start()
	{
		if (_id == 0) SetLock(false); else if (SecurePlayerPrefs.GetInt("faceAvailable_" + _id) != 1)
		{
			SetLock(true);
		}
		else
			SetLock(false);
	}
	
	override public void SetLock(bool isUnlocked)
	{
		base.SetLock(isUnlocked);
		foreach (GameObject obj in _objects)
		{
			obj.SetActive(isUnlocked);
		}
	}
	
	// Выбираем скин, который хотим установить
	public override void Click()
	{
		base.Click();
		
		// Если скин еще не открыт
		if (SecurePlayerPrefs.GetInt("faceAvailable_" + _id) != 1)
		{
			Application.OpenURL("https://www.instagram.com/squaredino/");
			OpenSkin();
			Select();
		}
	}
}
