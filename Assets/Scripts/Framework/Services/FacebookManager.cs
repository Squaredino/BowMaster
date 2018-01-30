using UnityEngine;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour {

	private void Awake()
	{
		// Код с сайта - https://developers.facebook.com/docs/unity/reference/current/FB.ActivateApp
		FB.Init(InitCallback, OnHideUnity);
		Debug.Log("FB.Init() called with " + FB.AppId);
	}
	
	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			Debug.Log("Initialized the Facebook SDK");
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Success - Check log for details");
		Debug.Log(string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown));
		Debug.Log("Is game shown: " + isGameShown);
	}
    
	// Unity will call OnApplicationPause(false) when an app is resumed
// from the background
	void OnApplicationPause (bool pauseStatus)
	{
		// Check the pauseStatus to see if we are in the foreground
		// or background
		if (!pauseStatus) {
			//app resume
			if (FB.IsInitialized) {
				FB.ActivateApp();
			} else {
				//Handle FB.Init
				FB.Init( () => {
					FB.ActivateApp();
				});
			}
		}
	}
}
