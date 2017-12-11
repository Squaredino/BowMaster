using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float despawnTimer;
    public int maxParticleLevel;
    public int particleLevel;

    private Game game;
    private Rigidbody2D rigidBody;
    private ParticleSystem particles;
    private ParticleSystem.MainModule main;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        particles = gameObject.GetComponent<ParticleSystem>();
        main = particles.main;
    }

    void Update()
    {
        if (rigidBody.simulated && rigidBody.velocity.y > 0 && particleLevel > 0)
        {
            if (!particles.isEmitting)
            {
                particleLevel = Mathf.Min(particleLevel, maxParticleLevel);

                var startSize = main.startSize;
                startSize.constant = particleLevel / 10f;
                main.simulationSpeed = 1 + particleLevel / 10f;

                particles.Play();
            }
        }
        else
        {
            if (particles.isEmitting)
            {
                particles.Stop();
            }
        }

        if (!game.gameBounds.Contains(transform.position))
        {
            gameObject.SetActive(false);
            game.TargetHit(false);
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
