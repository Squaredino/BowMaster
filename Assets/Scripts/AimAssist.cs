using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public float scaleModifier;

    private GameObject parentObj;
    private Renderer render;
    private Game game;
    private Vector3 tmpVector;

    void Start()
    {
        parentObj = new GameObject("AimAssistParent");
        transform.SetParent(parentObj.transform);
        render = gameObject.GetComponent<Renderer>();
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    void Update()
    {
        if (game.arrow != null)
        {
            render.enabled = true;
            transform.localPosition = Vector3.up * game.arrow.GetComponent<Renderer>().bounds.extents.y;
            parentObj.transform.position = game.arrow.transform.position;
            parentObj.transform.rotation = game.arrow.transform.rotation;
        }
        else
        {
            render.enabled = false;
        }

        tmpVector = transform.localScale;
        tmpVector.y = game.swipe.magnitude * scaleModifier;
        transform.localScale = tmpVector;
    }
}
