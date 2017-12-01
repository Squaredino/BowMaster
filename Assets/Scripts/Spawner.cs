using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Rect bounds;
    public float spawnInterval;

    private Game game;
    private float elapsedTime;

    private void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        elapsedTime = spawnInterval;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > spawnInterval)
        {
            Spawn();
            elapsedTime = 0;
        }
    }

    private void Spawn()
    {
        var newObj = Pool.Get(prefab);
        float x = Random.Range(bounds.xMin, bounds.xMax);
        float y = Random.Range(bounds.yMin, bounds.yMax);

        newObj.transform.position = new Vector2(x, y);
        newObj.transform.up = newObj.transform.position - (Vector3)game.archerPos; // rotate towards archer
    }
}
