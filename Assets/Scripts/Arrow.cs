using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public const float particleSizeMin = .5f, particleSizeGrow = .2f;
    public const float fadeInDuration = 0.1f, fadeOutDuration = 0.1f;

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
        if (rigidBody.simulated && particleLevel > 0)
        {
            if (!particles.isEmitting)
            {
                particleLevel = Mathf.Min(particleLevel, maxParticleLevel);

                var startSize = main.startSize;
                startSize.constant = particleSizeMin + particleLevel * particleSizeGrow;
                main.startSize = startSize;

                //main.simulationSpeed = 1 + particleLevel / 10f;

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

    private void OnEnable()
    {
        var scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, fadeInDuration);
    }

    public void Stop()
    {
        rigidBody.simulated = false;
        arrowHeadRigidBody.simulated = false;
    }

    public void Despawn()
    {
        transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
    }
}
