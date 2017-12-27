using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public const float fadeOutDuration = 0.15f;

    public float despawnTimer;
    public bool faceArcher;
    public float centerRadius;

    private Game game;
    private GameObject sprite;
    private Rigidbody2D rigidBody, bullseyeRigidBody;
    private Text text;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        sprite = transform.Find("Sprite").gameObject;
        rigidBody = GetComponent<Rigidbody2D>();
        bullseyeRigidBody = transform.Find("Bullseye").GetComponent<Rigidbody2D>();
        text = transform.Find("Sprite/Canvas/BullseyeText").GetComponent<Text>();
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
    }

    public void Stop()
    {
        rigidBody.simulated = false;
        bullseyeRigidBody.simulated = false;
    }

    public void Despawn()
    {
        sprite.transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(Deactivate);
    }

    private void Deactivate()
    { 
        text.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowBullseyeText()
    {
        text.gameObject.SetActive(true);
        text.text = game.GetBullseyeText();
        text.transform.DOPunchScale(Vector3.one, 0.2f);
    }
}
