using UnityEngine;

public class ScreenSkinsBtnTargets : MonoBehaviour {

	public void Click()
	{
		GlobalEvents<OnBtnArrowsHide>.Call(new OnBtnArrowsHide());
		GlobalEvents<OnBtnTargetsShow>.Call(new OnBtnTargetsShow());
	}
}
