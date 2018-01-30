using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    private const float PositionY = 2.5f;
    private const float PositionXSpace = 1f;
    private readonly List<GameObject> _activeArr = new List<GameObject>();
    private float _positionYSpace;
    public GameObject[] Clouds;

    // Use this for initialization
    private void Start()
    {
        float height = Camera.main.pixelHeight;
        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, height));
        _positionYSpace = bottomLeft.y - 0.5f;

        var go = AddObject();
        go.transform.position = new Vector3(-9.5f + Random.Range(-PositionXSpace, PositionXSpace),
            Random.Range(PositionY, _positionYSpace), 1f);
        go = AddObject();
        go.transform.position = new Vector3(0f + Random.Range(-PositionXSpace, PositionXSpace),
            Random.Range(PositionY, _positionYSpace), 1f);
        go = AddObject();
        go.transform.position = new Vector3(6.5f + Random.Range(-PositionXSpace, PositionXSpace),
            Random.Range(PositionY, _positionYSpace), 1f);

        StartCoroutine(SpawnCloud());
    }

    private IEnumerator SpawnCloud()
    {
        while (true)
        {
            AddObject();
            yield return new WaitForSeconds(22f + Random.value * 3.0f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        SpriteRenderer sprite;
        GameObject go;
        var i = 0;
        while (i < _activeArr.Count)
        {
            go = _activeArr[i];
            sprite = go.GetComponent<SpriteRenderer>();
            go.transform.position = new Vector3(go.transform.position.x - 0.005f, go.transform.position.y,
                go.transform.position.z);

            if (go.transform.position.x + sprite.bounds.size.x * 0.5f < bottomLeft.x)
            {
                _activeArr.Remove(go);
                Destroy(go);
                continue;
            }
            ++i;
        }
    }

    private GameObject AddObject()
    {
        var go = Instantiate(GetRandomBuilding(), new Vector3(), Quaternion.identity);
        var sprite = go.GetComponent<SpriteRenderer>();

        float width = Camera.main.pixelWidth;
        Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(width, 0));
        go.transform.position = new Vector3(
            bottomRight.x + sprite.bounds.size.x * 0.5f + Random.Range(0, PositionXSpace),
            Random.Range(PositionY, _positionYSpace), 1f);
        _activeArr.Add(go);
        return go;
    }

    private GameObject GetRandomBuilding()
    {
        return Clouds[(int) Mathf.Round(Random.value * (Clouds.Length - 1))];
    }
}