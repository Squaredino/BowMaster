using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public const float fadeOutDuration = 0.15f, jumpDuration = 0.1f;
    public const float jumpPowerNormal = 0.05f, jumpPowerBullseye = 0.1f;

    public float despawnTimer;
    public bool faceArcher;
    public Color hitColorNormal, hitColorBullseye;

    private Game game;
    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody, bullseyeRigidBody;
    private ParticleSystem particlesHitLite, particlesHitHeavy;
    private Text bullseyeText;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        bullseyeRigidBody = transform.Find("Bullseye").GetComponent<Rigidbody2D>();
        bullseyeText = transform.Find("Sprite/Canvas/BullseyeText").GetComponent<Text>();
        particlesHitLite = transform.Find("Particles/HitLite").gameObject.GetComponent<ParticleSystem>();
        particlesHitHeavy = transform.Find("Particles/HitHeavy").gameObject.GetComponent<ParticleSystem>();
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
        if (isBullseye) ShowBullseyeText();
        PlayHitAnimations(isBullseye);
        PlayHitParticles(isBullseye);
        Stop();
        StartCoroutine(Utils.DelayedAction(Despawn, despawnTimer));
    }

    private void PlayHitAnimations(bool isBullseye)
    {
        sprite.transform.DOLocalJump(Vector3.zero, isBullseye ? jumpPowerBullseye : jumpPowerNormal, 1, jumpDuration);
        sprite.DOColor(isBullseye ? hitColorBullseye : hitColorNormal, jumpDuration)
            .OnComplete(() => sprite.DOColor(Color.white, jumpDuration));
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
        sprite.transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(
            () =>
            {
                bullseyeText.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });
    }

    public void ShowBullseyeText()
    {
        bullseyeText.gameObject.SetActive(true);
        bullseyeText.text = game.GetBullseyeText();
        bullseyeText.transform.DOPunchScale(Vector3.one, 0.2f);
    }
}