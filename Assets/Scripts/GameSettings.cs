using PrefsEditor;
using UnityEngine;

public class GameSettings : MonoBehaviour {
	public static bool IsVibro; // Vibro
	
	private void Awake()
	{
		IsVibro = SecurePlayerPrefs.GetBool("GameSettings.Vibro", true);
	}

	private void OnEnable()
	{
		GlobalEvents<OnVibrate>.Happened += OnVibrate;
	}

	private void OnVibrate(OnVibrate obj)
	{
		if (IsVibro) Handheld.Vibrate();
	}

	public void ClickVibro()
	{
		IsVibro = !IsVibro;
		SecurePlayerPrefs.SetBool("GameSettings.Vibro", IsVibro);
		if (IsVibro) Handheld.Vibrate();
	}
}
