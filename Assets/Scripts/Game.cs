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
                startTouchPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
#else
        { 
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(arrow.transform.position);
                startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

                swipe = endTouchPos - startTouchPos;

                if (swipe.y < 0)
                {
                    var rigidBody = arrow.GetComponent<Rigidbody2D>();
                    rigidBody.AddForce(-swipe * 300);
                    rigidBody.simulated = true;
                }
            }
        }
    }

    public void SpawnArrow()
    {
        Destroy(arrow);

        arrow = Instantiate(arrowPrefab, archerPos, transform.rotation);
        arrow.transform.position += (Vector3)arrowArcherOffset;
    }

    public void SpawnTarget()
    {
        int x = UnityEngine.Random.Range(-2, 2);
        int y = UnityEngine.Random.Range(-2, 2);
        Vector2 targetPos = new Vector2(x, Math.Max(y, targetMinY));
        Destroy(target);
        target = Instantiate(targetPrefab, targetPos, transform.rotation);
    }

    public IEnumerator DelayedAction(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
