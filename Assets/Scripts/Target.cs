using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float despawnTimer;
    public bool faceArcher;
    public float centerRadius;

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

    private void OnEnable()
    {
        if (rigidBody)
        {
            rigidBody.simulated = true;
        }
    }

    public void Stop()
    {
        rigidBody.simulated = false;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
