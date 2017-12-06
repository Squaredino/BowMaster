using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Rect bounds;
    public float spawnInterval;
    public SpawnerStrategy.SpawnStrategy spawnStrategy;

    private float elapsedTime;
    Movement movement;

    private void Start()
    {
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
        var objList = spawnStrategy(GetObject);

        if (objList != null)
        {
            foreach (var obj in objList) // [0; 1] -> [xMin; xMax]
            {
                Vector2 pos = obj.transform.position;
                pos.x = pos.x * bounds.width + bounds.xMin;
                pos.y = pos.y * bounds.height + bounds.yMin;
                obj.transform.position = pos;
                movement = obj.GetComponent<Movement>();
                if (movement != null)
                {
                    for (int i = 0; i < movement.waypoints.Count; i++)
                    {
                        Vector2 point = movement.waypoints[i];
                        point.x = point.x * bounds.width + bounds.xMin;
                        point.y = point.y * bounds.height + bounds.yMin;
                        movement.waypoints[i] = point;
                    }
                }
            }
        }
    }

    public GameObject GetObject()
    {
        return Pool.Get(prefab);
    }
}
