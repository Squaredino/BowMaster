using UnityEngine;

public class PointsBallonManager : MonoBehaviour
{
	[SerializeField] private GameObject _pointsBalloon;
//
//	private void Awake()
//	{
//		_canvas = GetComponent<Canvas>();
//	}

	public void Add(int value, Vector3 position)
	{
		GameObject go = Instantiate(_pointsBalloon);
		go.transform.SetParent(transform, false);
		go.transform.position = position + new Vector3(0f, 40f,0f);
		PointsBalloon pb = go.GetComponent<PointsBalloon>();
		pb.SetCount(value);
	}
}
