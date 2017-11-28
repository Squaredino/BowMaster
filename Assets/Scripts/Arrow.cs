using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
        collision.otherRigidbody.simulated = false;
        StartCoroutine(game.DelayedAction(game.SpawnArrow, 1));
        StartCoroutine(game.DelayedAction(game.SpawnTarget, 1));
    }
}
