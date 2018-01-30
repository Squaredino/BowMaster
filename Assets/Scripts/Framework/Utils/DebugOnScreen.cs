using UnityEngine;
using UnityEngine.UI;

public class DebugOnScreen : MonoBehaviour
{
	[SerializeField] private Text text;

	private void OnEnable()
	{
		GlobalEvents<OnDebugLog>.Happened += OnDebugLog;
	}

	private void OnDebugLog(OnDebugLog obj)
	{
		text.text += obj.message + "\n";
	}
}
