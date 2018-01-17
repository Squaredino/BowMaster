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
    private bool isOutOfBounds;
    private Game game;
    private GameObject arrowHead, sprite;
    private Line line;
    private Renderer spriteRenderer;
    private Rigidbody2D rigidBody, arrowHeadRigidBody;
    private ParticleSystem particlesLite, particlesHeavy;
    private ParticleSystem particlesNormalHit, particlesBullseyeHit;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        arrowHead = transform.Find("Arrowhead").gameObject;
        line = transform.Find("Line").GetComponent<Line>();
        sprite = transform.Find("Sprite").gameObject;
        spriteRenderer = sprite.GetComponent<Renderer>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        arrowHeadRigidBody = arrowHead.GetComponent<Rigidbody2D>();
        particlesLite = transform.Find("TrailLite").gameObject.GetComponent<ParticleSystem>();
        particlesHeavy = transform.Find("TrailHeavy").gameObject.GetComponent<ParticleSystem>();
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
        else if (particleLevel < 2)
        {
            if (!particlesLite.isEmitting)
                particlesLite.Play();
            if (particlesHeavy.isEmitting)
                particlesHeavy.Stop();
        }
        else if (particleLevel >= 2)
        {
            if (particlesLite.isEmitting)
                particlesLite.Stop();
            if (!particlesHeavy.isEmitting)
                particlesHeavy.Play();
        }

        if (rigidBody.simulated)
        {
            line.EndPoint = sprite.transform.position;

            if (!game.gameBounds.Contains(spriteRenderer.bounds.max) ||
                !game.gameBounds.Contains(spriteRenderer.bounds.min))
            {
                if (!isOutOfBounds)
                {
                    game.TargetMiss(transform.position);
                    isOutOfBounds = true;
                }

                if (!game.gameBounds.Contains(spriteRenderer.bounds.max) &&
                    !game.gameBounds.Contains(spriteRenderer.bounds.min))
                {
                    Stop();
                    Despawn();
                }
            }
        }
    }

    void OnEnable()
    {
        if (line)
        {
            line.StartPoint = sprite.transform.position;
            line.EndPoint = sprite.transform.position;
        }

        if (particlesHeavy)
        {
            particlesLite.transform.localPosition = Vector3.up * 0.2f;
            particlesHeavy.transform.localPosition = Vector3.up * 0.2f;
        }
    }

    public void SetParticleLevel(int level)
    {
        particleLevel = Mathf.Min(maxParticleLevel, Mathf.Max(0, level));
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
        line.StartPoint = sprite.transform.position;
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
        isOutOfBounds = false;
        sprite.transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
    }
}