using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float despawnTimer = 0.5f;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidBody.simulated && rigidBody.velocity.y < 0)
        {
            rigidBody.simulated = false;
            StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
        }
    }

    private void OnBecameInvisible()
    {
        //gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Target"))
        {
            StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
        }
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
