using PrefsEditor;
using UnityEngine;

public class GameSettings : MonoBehaviour {
	public static bool IsVibro; // Vibro
	
	private void Awake()
	{
		IsVibro = SecurePlayerPrefs.GetBool("GameSettings.Vibro", true);
	}

	public void ClickVibro()
	{
		IsVibro = !IsVibro;
		SecurePlayerPrefs.SetBool("GameSettings.Vibro", IsVibro);
		if (IsVibro) Handheld.Vibrate();
	}
}
