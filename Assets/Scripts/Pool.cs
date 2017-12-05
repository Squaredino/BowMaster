using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private const int INITIAL_SIZE = 10;
    private const int GROW_SIZE = 5;

    private static Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();

    private GameObject prefab;
    private List<GameObject> objects = new List<GameObject>();
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static GameObject Get(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            var newPool = new GameObject(prefab.name + "Pool").AddComponent<Pool>();
            newPool.Initialize(prefab);
            pools.Add(prefab, newPool);
        }

        while (pools[prefab].availableObjects.Count == 0)
        {
            foreach (var obj in pools[prefab].objects)
            {
                if (!obj.activeSelf)
                {
                    obj.transform.position = Vector3.zero;
                    obj.transform.rotation = Quaternion.identity;
                    obj.transform.localScale = Vector3.one;
                    pools[prefab].availableObjects.Enqueue(obj);
                }
            }
            if (pools[prefab].availableObjects.Count == 0)
            {
                pools[prefab].Grow();
            }
        }

        var returnedObj = pools[prefab].availableObjects.Dequeue();
        returnedObj.SetActive(true);
        return returnedObj;
    }

    private void Initialize(GameObject prefab)
    {
        this.prefab = prefab;
        Grow(INITIAL_SIZE);
    }

    private void Grow(int growSize = GROW_SIZE)
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
