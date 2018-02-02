using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private const int INITIAL_SIZE = 2;
    private const int GROW_SIZE = 2;

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
                    ResetObj(obj);

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

    private static void ResetObj(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            child.transform.position = Vector3.zero;
            child.transform.rotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }
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
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    public static List<GameObject> GetActiveObjects(GameObject prefab)
    {
        return pools[prefab].objects.Where(x => x.activeSelf).ToList();
    }
}