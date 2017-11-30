using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Game game;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.simulated = false;
        StartCoroutine(game.DelayedAction(game.SpawnTarget, 1));
    }
}
