﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float despawnTimer;
    public bool faceArcher;

    private Game game;
    private Rigidbody2D rigidBody;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (faceArcher)
        {
            transform.up = transform.position - (Vector3)game.archerPos; // rotate towards archer
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Arrow"))
        {
            collision.rigidbody.simulated = false;
            collision.otherRigidbody.simulated = false;
            StartCoroutine(Utils.DelayedAction(Despawn, 0.5f));
        }
    }

    private void OnBecameVisible()
    {
        rigidBody.simulated = true;
    }

    private void OnEnable()
    {
        StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
