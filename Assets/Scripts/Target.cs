using DG.Tweening;
using UnityEngine;

public class Target : MonoBehaviour
{
    public const float fadeOutDuration = 0.15f, jumpDuration = 0.1f;
    public const float jumpPowerNormal = 0.05f, jumpPowerBullseye = 0.1f;
    [SerializeField] private GameObject _visual;
    [SerializeField] private SpriteRenderer[] sprites = new SpriteRenderer[2];
    public float despawnTimer;
    public bool faceArcher;
    public Color hitColorNormal, hitColorBullseye;

    private Game game;
    
    private Rigidbody2D rigidBody, bullseyeRigidBody;
    private ParticleSystem particlesHitLite, particlesHitHeavy;
    private Movement movement;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        rigidBody = GetComponent<Rigidbody2D>();
        bullseyeRigidBody = transform.Find("Bullseye").GetComponent<Rigidbody2D>();
        particlesHitLite = transform.Find("Particles/HitLite").gameObject.GetComponent<ParticleSystem>();
        particlesHitHeavy = transform.Find("Particles/HitHeavy").gameObject.GetComponent<ParticleSystem>();
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (faceArcher)
        {
            transform.up = transform.position - (Vector3) game.arrowPos; // rotate towards archer
        }
    }

    private void OnEnable()
    {
        if (rigidBody)
        {
            rigidBody.simulated = true;
            bullseyeRigidBody.simulated = true;
        }
    }

    public void OnHit(bool isBullseye = false)
    {
        movement.Stop();
        PlayHitAnimations(isBullseye);
        PlayHitParticles(isBullseye);
        Stop();
        StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
    }

    private void PlayHitAnimations(bool isBullseye)
    {
        _visual.transform.DOLocalJump(Vector3.zero, isBullseye ? jumpPowerBullseye : jumpPowerNormal, 1, jumpDuration);
        foreach (SpriteRenderer _spr in sprites)
        {
            _spr.DOColor(isBullseye ? hitColorBullseye : hitColorNormal, jumpDuration)
                .OnComplete(() => _spr.DOColor(Color.white, jumpDuration));
        }
    }

    public void PlayHitParticles(bool isBullseye)
    {
        if (isBullseye)
            particlesHitHeavy.Play();
        else
            particlesHitLite.Play();
    }

    public void Stop()
    {
        rigidBody.simulated = false;
        bullseyeRigidBody.simulated = false;
    }

    public void Despawn()
    {
        _visual.transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(
            () =>
            {
                gameObject.SetActive(false);
            });
    }
}