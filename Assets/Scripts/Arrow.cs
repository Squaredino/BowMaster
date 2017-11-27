using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 oldPos, newPos;
    private bool isMoving = false;
    private float tmpTime = 0.0f;

    void Start()
    {
        oldPos = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.Lerp(oldPos, newPos, tmpTime);
            tmpTime += Time.deltaTime / (Vector2.Distance(oldPos, newPos) / moveSpeed);
            if (tmpTime >= 1)
            {
                isMoving = false;
                tmpTime = 0.0f;
            }
        }
    }

    public void MoveTo(Vector2 newPos)
    {
        if (!isMoving)
        {
            oldPos = transform.position;
            this.newPos = newPos;
            isMoving = true;
        }
    }
}
