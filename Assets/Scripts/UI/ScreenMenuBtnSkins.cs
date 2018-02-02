using UnityEngine;

public class ScreenMenuBtnSkins : MonoBehaviour {

	public void Click()
	{
		GlobalEvents<OnScreenSkinsShow>.Call(new OnScreenSkinsShow());
	}
}
