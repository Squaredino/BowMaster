using UnityEngine;

public class ScreenSkinsBtnArrows : MonoBehaviour {

	public void Click()
	{
		GlobalEvents<OnBtnTargetsHide>.Call(new OnBtnTargetsHide());
		GlobalEvents<OnBtnArrowsShow>.Call(new OnBtnArrowsShow());
	}
}
