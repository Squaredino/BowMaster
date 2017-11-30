using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private const int DEFAULT_INITIAL_SIZE = 10;
    private const int DEFAULT_GROW_SIZE = 5;

    private static Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();

    public int initialPoolSize = DEFAULT_INITIAL_SIZE;
    public int growSize = DEFAULT_GROW_SIZE;

    private GameObject prefab;
    private List<GameObject> objects = new List<GameObject>();
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static GameObject Get(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            var newPool = new GameObject(prefab.name + "Pool").AddComponent<Pool>();
            newPool.Setup(prefab);
            pools.Add(prefab, newPool);
        }
        if (pools[prefab].availableObjects.Count == 0)
        {
            foreach (var obj in pools[prefab].objects)
            {
                if (!obj.activeSelf)
                {
                    pools[prefab].availableObjects.Enqueue(obj);
                }
            }
            if (pools[prefab].availableObjects.Count == 0)
            {
                pools[prefab].GrowPool();
            }
        }

        var returnedObj = pools[prefab].availableObjects.Dequeue();
        returnedObj.SetActive(true);
        return returnedObj;
    }

    private void Setup(GameObject prefab)
    {
        this.prefab = prefab;

        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(this.prefab);
            obj.transform.parent = transform;
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    private void GrowPool()
    {
        for (int i = 0; i < growSize; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = transform;
            obj.SetActive(false);
            objects.Add(obj);
        }
    }
}
