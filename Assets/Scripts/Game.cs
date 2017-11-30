using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject arrowPrefab, targetPrefab;
    public Vector2 archerPos;
    public Vector2 arrowArcherOffset;
    public float targetMinY;
    public float minForce, maxForce;

    private GameObject arrow, target;
    private Vector2 startTouchPos, endTouchPos, swipe;

    void Start()
    {
        SpawnArrow();
        SpawnTarget();
    }

    void Update()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
            }
            swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Moved)
            {
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (touch.phase == TouchPhase.Ended)
            {
#else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0))
            {
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (Input.GetMouseButtonUp(0))
            {
#endif
                if (swipe.y > 0)
                {
                    var rigidBody = arrow.GetComponent<Rigidbody2D>();
                    rigidBody.AddForce(swipe * 300);
                    rigidBody.simulated = true;
                }
            }
        }
    }

    public void SpawnArrow()
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
        
        arrow = Pool.Get(arrowPrefab);
        arrow.transform.position = archerPos + arrowArcherOffset;
    }

    public void SpawnTarget()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
        
        target = Pool.Get(targetPrefab);

        int x = UnityEngine.Random.Range(-2, 2);
        int y = UnityEngine.Random.Range(-2, 2);
        target.transform.position = new Vector2(x, Math.Max(y, targetMinY));
        target.transform.up = target.transform.position - (Vector3)archerPos; // rotate towards archer
    }

    public IEnumerator DelayedAction(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
