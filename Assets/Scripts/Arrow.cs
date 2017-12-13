﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float despawnTimer;
    public int maxParticleLevel;
    public int particleLevel;

    private Game game;
    private GameObject arrowHead;
    private Rigidbody2D rigidBody, arrowHeadRigidBody;
    private ParticleSystem particles;
    private ParticleSystem.MainModule main;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        arrowHead = transform.GetChild(0).gameObject;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        arrowHeadRigidBody = arrowHead.GetComponent<Rigidbody2D>();
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
            Stop();
            gameObject.SetActive(false);
            game.TargetHit(false);
        }
    }

    public void Shoot(Vector2 force)
    {
        rigidBody.simulated = true;
        arrowHeadRigidBody.simulated = true;
        rigidBody.AddForce(force);
    }

    public void Stop()
    {
        rigidBody.simulated = false;
        arrowHeadRigidBody.simulated = false;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
