using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public List<Vector2> waypoints;
    public float speed;

    private Rigidbody2D rigidBody;
    private int currWaypoint = 0;
    private float minDist = 0.2f;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (waypoints.Any())
        {
            rigidBody.MovePosition(rigidBody.position + (waypoints[currWaypoint] - rigidBody.position).normalized * speed * Time.deltaTime);
            if (Vector2.Distance(rigidBody.position, waypoints[currWaypoint]) < minDist)
            {
                currWaypoint = currWaypoint + 1 >= waypoints.Count ? 0 : currWaypoint + 1;
            }
        }
    }

    private void OnBecameVisible()
    {
        if (waypoints.Any())
        {
            currWaypoint = 0;
            transform.position = waypoints[currWaypoint++];
        }
    }
}
