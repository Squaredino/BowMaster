using UnityEngine;
//using VoxelBusters.NativePlugins;

public class Share : MonoBehaviour
{
	private string _gifName;
	private bool _isGift;

	private void OnEnable()
    {
        GlobalEvents<OnBtnShareClick>.Happened += OnBtnShareClick;
//		GlobalEvents<OnBtnShareGifClick>.Happened += OnBtnShareGifClick;
		GlobalEvents<OnGifSetName>.Happened += OnGifSetName;
    }

	private void OnGifSetName(OnGifSetName obj) {
		_gifName = obj.FilePathWithName;
	}

//	private void OnBtnShareGifClick(OnBtnShareGifClick obj)
//	{
//		ShareGifClick();
//	}

	public void ShareGifClick()
    {
        var shareLink = "itms-apps://itunes.apple.com/app/id1288514456";

        var shareText = "Wow! I Just Scored [" + PrefsManager.GameBestScore +
                        "] in #SlimRing! Can You Beat Me? @Squaredino " + shareLink;

		ShareImageAtPathUsingShareSheet(shareText, _gifName + ".gif");
    }

	private void OnBtnShareClick(OnBtnShareClick obj)
	{
		ShareClick(true);
	}

	public void ShareClick(bool isGift = false)
	{
		_isGift = isGift;
		var shareLink = "itms-apps://itunes.apple.com/app/id1288514456";

		var shareText = "Wow! I Just Scored [" + PrefsManager.GameBestScore +
                        "] in #SlimRing! Can You Beat Me? @Squaredino " + shareLink;


		var screenShotPath = Application.persistentDataPath + "/promo1.jpg";

		if (Random.value > 0.5f) screenShotPath = Application.persistentDataPath + "/promo2.jpg";

		ShareImageAtPathUsingShareSheet(shareText, screenShotPath);
	}

    private void ShareImageAtPathUsingShareSheet(string shareText, string screenShotPath)
	{
		// Create share sheet
//		ShareSheet shareSheet = new ShareSheet ();

//		shareSheet.Text = shareText;
//		shareSheet.AttachImageAtPath (screenShotPath);

		// Show composer
//		NPBinding.Sharing.ShowView (shareSheet, FinishedSharing);

		Debug.Log ("Finished sharing");
	}

//	void FinishedSharing (eShareResult result){
//		Debug.Log("Share Result = " + result);
//		if(_isGift)
//		GlobalEvents<OnGifShared>.Call (new OnGifShared ());
//	}
}