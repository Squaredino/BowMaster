using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnerStrategy
{
    public delegate GameObject GetObject();
    public delegate List<GameObject> SpawnStrategy(GetObject getMethod);

    public static List<GameObject> Simple(GetObject getMethod)
    {
        List<GameObject> objects = new List<GameObject>();
        GameObject obj = getMethod();

        obj.transform.position = new Vector2(Random.value, Random.value);
        objects.Add(obj);

        return objects;
    }

    public static List<GameObject> HalfSized(GetObject getMethod)
    {
        List<GameObject> objects = new List<GameObject>();
        GameObject obj = getMethod();

        obj.transform.position = new Vector2(Random.value, Random.value);
        obj.transform.localScale = Vector3.one * 0.5f;
        objects.Add(obj);

        return objects;
    }

    public static List<GameObject> Row(GetObject getMethod)
    {
        List<GameObject> objects = new List<GameObject>();
        GameObject obj;
        int count = 5;

        float yOffset = Random.value;
        for (int i = 0; i < count; i++)
        {
            obj = getMethod();
            obj.transform.position = new Vector2(1 / (float)count * i, yOffset);
            objects.Add(obj);
        }

        return objects;
    }

    public static List<GameObject> SimpleMoving(GetObject getMethod)
    {
        List<GameObject> objects = new List<GameObject>();
        GameObject obj = getMethod();
        Movement movement = obj.GetComponent<Movement>();

        List<Vector2> waypoints = new List<Vector2>();
        float yOffset = Random.value;
        waypoints.Add(new Vector2(Random.value, yOffset));
        waypoints.Add(new Vector2(Random.value, yOffset));
        if (movement != null)
        {
            movement.waypoints = waypoints;
        }
        else
        {
            obj.transform.position = waypoints[0];
        }
        objects.Add(obj);

        return objects;
    }
}
