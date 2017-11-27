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

    private GameObject arrow, target;
    private Vector2 touchPos;
    private bool isArrowDespawned = true;
    private bool isTargetDespawned = true;

    void Start()
    {

    }

    void Update()
    {
        if (isArrowDespawned)
        {
            Destroy(arrow);

            arrow = Instantiate(arrowPrefab, archerPos, transform.rotation);
            arrow.transform.position += (Vector3)arrowArcherOffset;

            isArrowDespawned = false;
        }

        if (isTargetDespawned)
        {
            Destroy(target);

            SpawnTarget();

            isTargetDespawned = false;
        }

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
        if (Input.GetMouseButton(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
            if (touchPos.y > arrow.transform.position.y)
            {
                arrow.GetComponent<Arrow>().MoveTo(touchPos);
            }
        }

        if (arrow.GetComponent<BoxCollider2D>().bounds.Intersects(target.GetComponent<CircleCollider2D>().bounds))
        {
            isArrowDespawned = true;
            isTargetDespawned = true;
        }
    }

    public void SpawnTarget()
    {
        int x = UnityEngine.Random.Range(-2, 2);
        int y = UnityEngine.Random.Range(-2, 2);
        Vector2 targetPos = new Vector2(x, Math.Max(y, targetMinY));
        target = Instantiate(targetPrefab, targetPos, transform.rotation);
    }
}
