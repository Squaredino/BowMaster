using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Rect bounds, scoreBounds;
    public float spawnInterval;
    public SpawnerStrategy.SpawnStrategy spawnStrategy = SpawnerStrategy.Simple;
    public Vector3 scale;

    private Vector2 tmpVector;
    private Movement tmpMovement;
    private float elapsedTime;

    private void Start()
    {
        elapsedTime = spawnInterval;
    }

    private void Update()
    {
        if (spawnInterval > 0f)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > spawnInterval)
            {
                Spawn();
                elapsedTime = 0f;
            }
        }
    }

    public void Spawn()
    {
        var objList = spawnStrategy(GetObject);

        if (objList != null)
        {
            foreach (var obj in objList) // [0; 1] -> [xMin; xMax]
            {
                tmpVector = obj.transform.position;
                tmpVector.x = tmpVector.x * bounds.width + bounds.xMin;
                tmpVector.y = tmpVector.y * bounds.height + bounds.yMin;
                if (scoreBounds.Contains(tmpVector))
                    tmpVector.x = tmpVector.x > scoreBounds.center.x ? scoreBounds.x : scoreBounds.xMax;
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

                obj.transform.localScale = scale;
            }
        }
    }

    public List<GameObject> SpawnedObjects()
    {
        return Pool.ActiveObjects(prefab).ToList();
    }

    public GameObject GetObject()
    {
        return Pool.Get(prefab);
    }
}
