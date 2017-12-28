using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public const float particleSizeMin = .5f, particleSizeGrow = .2f;
    public const float fadeOutDuration = 0.1f;

    public float despawnTimer;
    public int maxParticleLevel;

    private int particleLevel;
    private Game game;
    private GameObject arrowHead, sprite;
    private Rigidbody2D rigidBody, arrowHeadRigidBody;
    private ParticleSystem particlesLite, particlesHeavy;
    private ParticleSystem particlesNormalHit, particlesBullseyeHit;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        arrowHead = transform.Find("Arrowhead").gameObject;
        sprite = transform.Find("Sprite").gameObject;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        arrowHeadRigidBody = arrowHead.GetComponent<Rigidbody2D>();
        particlesLite = transform.Find("Particles/TrailLite").gameObject.GetComponent<ParticleSystem>();
        particlesHeavy = transform.Find("Particles/TrailHeavy").gameObject.GetComponent<ParticleSystem>();
        particlesNormalHit = transform.Find("Particles/NormalHit").gameObject.GetComponent<ParticleSystem>();
        particlesBullseyeHit = transform.Find("Particles/BullseyeHit").gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particleLevel <= 0)
        {
            if (particlesLite.isEmitting)
                particlesLite.Stop();
            if (particlesHeavy.isEmitting)
                particlesHeavy.Stop();
        }
        else if (particleLevel < 3)
        {
            if (!particlesLite.isEmitting)
                particlesLite.Play();
            if (particlesHeavy.isEmitting)
                particlesHeavy.Stop();
        }
        else if (particleLevel >= 3)
        {
            if (particlesLite.isEmitting)
                particlesLite.Stop();
            if (!particlesHeavy.isEmitting)
                particlesHeavy.Play();
        }

        if (!game.gameBounds.Contains(transform.position))
        {
            Stop();
            gameObject.SetActive(false);
            game.TargetMiss(transform.position);
        }
    }

    public void SetParticleLevel(int particleLevel)
    {
        this.particleLevel = Mathf.Min(maxParticleLevel, Mathf.Max(0, particleLevel));
    }

    public void PlayHitParticles(bool isBullseye = false)
    {
        if (isBullseye)
        {
            particlesBullseyeHit.transform.localPosition = Vector2.up * 0.64f;
            particlesBullseyeHit.Play();
        }
        else
        {
            particlesNormalHit.transform.localPosition = Vector2.up * 0.64f;
            particlesNormalHit.Play();
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
        particleLevel = 0;
    }

    public void Despawn()
    {
        particlesLite.Clear();
        particlesHeavy.Clear();
        sprite.transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
    }
}
