using UnityEngine;

public class ScreenSkinsBtnBack : MonoBehaviour {

	public void Click()
	{
		GlobalEvents<OnScreenSkinsHide>.Call(new OnScreenSkinsHide());
	}
}
