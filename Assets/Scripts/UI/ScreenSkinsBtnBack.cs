using UnityEngine;

public class ScreenSkinsBtnBack : MonoBehaviour {

	public void Click()
	{
		GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable {Flag = true});
	}
}
