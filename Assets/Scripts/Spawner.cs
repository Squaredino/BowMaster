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

    private Vector2 tmpVector;
    private Movement tmpMovement;
    private float elapsedTime;

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
                tmpVector = obj.transform.position;
                tmpVector.x = tmpVector.x * bounds.width + bounds.xMin;
                tmpVector.y = tmpVector.y * bounds.height + bounds.yMin;
                obj.transform.position = tmpVector;

                tmpMovement = obj.GetComponent<Movement>();
                if (tmpMovement != null)
                {
                    for (int i = 0; i < tmpMovement.waypoints.Count; i++)
                    {
                        tmpVector = tmpMovement.waypoints[i];
                        tmpVector.x = tmpVector.x * bounds.width + bounds.xMin;
                        tmpVector.y = tmpVector.y * bounds.height + bounds.yMin;
                        tmpMovement.waypoints[i] = tmpVector;
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
