using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public const float fadeInDuration = 0.2f, fadeOutDuration = 0.2f;

    public float despawnTimer;
    public bool faceArcher;
    public float centerRadius;

    private Game game;
    private Rigidbody2D rigidBody, bullseyeRigidBody;
    private Text text;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        rigidBody = GetComponent<Rigidbody2D>();
        bullseyeRigidBody = transform.Find("Bullseye").GetComponent<Rigidbody2D>();
        text = transform.Find("Canvas/BullseyeText").GetComponent<Text>();
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
            bullseyeRigidBody.simulated = true;
        }

        var scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, fadeInDuration);
    }

    public void Stop()
    {
        rigidBody.simulated = false;
        bullseyeRigidBody.simulated = false;
    }

    public void Despawn()
    {
        transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(Deactivate);
    }

    private void Deactivate()
    { 
        text.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowBullseyeText()
    {
        text.gameObject.SetActive(true);
        text.transform.DOPunchScale(Vector3.one, 0.2f);
    }
}
