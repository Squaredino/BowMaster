using UnityEngine;

public class ScreenGame : MonoBehaviour
{
	[SerializeField] private GameObject _reviveScreen;
	
	private void OnEnable()
	{
		GlobalEvents<OnScreenReviveShow>.Happened += OnScreenReviveShow;
	}

	private void OnScreenReviveShow(OnScreenReviveShow obj)
	{
		GameObject go = Instantiate(_reviveScreen);
		go.transform.SetParent(transform);
	}
}
