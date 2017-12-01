using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Arrow"))
        {
            collision.rigidbody.simulated = false;
            StartCoroutine(Utils.DelayedAction(Despawn, 0.5f));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Utils.DelayedAction(Despawn, 5f));
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
