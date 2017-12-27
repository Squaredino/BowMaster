using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowhead : MonoBehaviour
{
    private Arrow arrow;
    private Game game;

    void Start()
    {
        arrow = transform.parent.GetComponent<Arrow>();
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.rigidbody.simulated || !collision.otherRigidbody.simulated)
        {
            return;
        }

        Target target = null;
        bool bullseye = false;

        if (collision.gameObject.name.Contains("Target"))
        {
            target = collision.gameObject.GetComponent<Target>();
            bullseye = false;
        }
        else if (collision.gameObject.name.Contains("Bullseye"))
        {
            target = collision.transform.parent.GetComponent<Target>();
            target.ShowBullseyeText();
            bullseye = true;
        }

        if (target != null)
        {
            target.Stop();
            arrow.Stop();
            StartCoroutine(Utils.DelayedAction(target.Despawn, target.despawnTimer));
            StartCoroutine(Utils.DelayedAction(arrow.Despawn, arrow.despawnTimer));
            game.TargetHit(bullseye);
        }
    }
}
