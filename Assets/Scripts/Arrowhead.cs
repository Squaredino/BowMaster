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
        var isBullseye = false;

        if (collision.gameObject.name.Contains("Target"))
        {
            target = collision.gameObject.GetComponent<Target>();
        }
        else if (collision.gameObject.name.Contains("Bullseye"))
        {
            target = collision.transform.parent.GetComponent<Target>();
            isBullseye = true;
        }

        if (target != null)
        {
            target.OnHit(isBullseye);
            arrow.OnHit(isBullseye);

            game.TargetHit(isBullseye);
        }
    }
}
