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

    private Gameplay _gameplay;
    
    private Rigidbody2D rigidBody, bullseyeRigidBody;
    private ParticleSystem particlesHitLite, particlesHitHeavy;
    private Movement movement;

    void Start()
    {
        _gameplay = GameObject.Find("Game").GetComponent<Gameplay>();
        rigidBody = GetComponent<Rigidbody2D>();
        bullseyeRigidBody = transform.Find("Bullseye").GetComponent<Rigidbody2D>();
        particlesHitLite = transform.Find("Particles/HitLite").gameObject.GetComponent<ParticleSystem>();
        particlesHitHeavy = transform.Find("Particles/HitHeavy").gameObject.GetComponent<ParticleSystem>();
        movement = GetComponent<Movement>();
        
//        LoadSkin(ScreenSkins.CurrentTargetId);
    }
    
    private void OnEnable()
    {
        GlobalEvents<OnChangeTargetSkin>.Happened += OnChangeTargetSkin;
        if (rigidBody)
        {
            rigidBody.simulated = true;
            bullseyeRigidBody.simulated = true;
        }
    }

    private void OnDisable()
    {
        GlobalEvents<OnChangeTargetSkin>.Happened -= OnChangeTargetSkin;
    }

    private void OnChangeTargetSkin(OnChangeTargetSkin obj)
    {
        LoadSkin(obj.Id);
    }

    private void LoadSkin(int objId)
    {
        // Верхний спрайт
        if (sprites[0].sprite) Resources.UnloadAsset(sprites[0].sprite);
        sprites[0].sprite = Resources.Load<Sprite>("Gfx/Targets/target_" + (objId + 1) + "_Up");
        // Нижний спрайт
        if (sprites[1].sprite) Resources.UnloadAsset(sprites[1].sprite);
        sprites[1].sprite = Resources.Load<Sprite>("Gfx/Targets/target_" + (objId + 1) + "_Down");
    }

    void Update()
    {
        if (faceArcher)
        {
            transform.up = transform.position - (Vector3) _gameplay.arrowPos; // rotate towards archer
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