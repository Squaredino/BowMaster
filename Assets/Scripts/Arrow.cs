using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public const float fadeOutDuration = 0.1f;

    public float despawnTimer;
    public int maxParticleLevel;

    private int particleLevel;
    private bool isOutOfBounds;
    private Gameplay _gameplay;
    private GameObject arrowHead;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rigidBody, arrowHeadRigidBody;
    private ParticleSystem particlesLite, particlesHeavy;
    private ParticleSystem particlesNormalHit, particlesBullseyeHit;

    void Start()
    {
        _gameplay = GameObject.Find("Game").GetComponent<Gameplay>();
        arrowHead = transform.Find("Arrowhead").gameObject;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        arrowHeadRigidBody = arrowHead.GetComponent<Rigidbody2D>();
        particlesLite = transform.Find("TrailLite").gameObject.GetComponent<ParticleSystem>();
        particlesHeavy = transform.Find("TrailHeavy").gameObject.GetComponent<ParticleSystem>();
        particlesNormalHit = transform.Find("Particles/NormalHit").gameObject.GetComponent<ParticleSystem>();
        particlesBullseyeHit = transform.Find("Particles/BullseyeHit").gameObject.GetComponent<ParticleSystem>();
//        LoadSkin(ScreenSkins.CurrentFaceId);
    }

    private void OnEnable()
    {
        GlobalEvents<OnChangeSkin>.Happened += OnChangeSkin;
    }
    
    private void OnDisable()
    {
        GlobalEvents<OnChangeSkin>.Happened -= OnChangeSkin;
    }

    private void OnChangeSkin(OnChangeSkin obj)
    {
        LoadSkin(obj.Id);
    }

    private void LoadSkin(int objId)
    {
        if (_spriteRenderer.sprite) Resources.UnloadAsset(_spriteRenderer.sprite);
        _spriteRenderer.sprite = Resources.Load<Sprite>("Gfx/Arrows/arrow_" + (objId + 1));
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

        SetParticlePositions();

        if (rigidBody.simulated)
        {
            if (!_gameplay.gameBounds.Contains(_spriteRenderer.bounds.max) ||
                !_gameplay.gameBounds.Contains(_spriteRenderer.bounds.min))
            {
                if (!isOutOfBounds)
                {
                    _gameplay.TargetMiss(transform.position);
                    isOutOfBounds = true;
                }

                if (!_gameplay.gameBounds.Contains(_spriteRenderer.bounds.max) &&
                    !_gameplay.gameBounds.Contains(_spriteRenderer.bounds.min))
                {
                    Invoke("Remove", 0.25f);
                }
            }
        }
    }

    private void SetParticlePositions()
    {
        if (particlesLite && particlesHeavy)
        {
            particlesLite.transform.localPosition = Vector3.down * 0.4f;
            particlesHeavy.transform.localPosition = Vector3.down * 0.4f;
        }
    }

    public void OnHit(bool isBullseye = false)
    {
        PlayHitParticles(isBullseye);
        Stop();
        StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
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
        rigidBody.AddForce(force);
    }

    private void Remove()
    {
        Stop();
        Despawn();
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
        transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
    }
}