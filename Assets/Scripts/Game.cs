using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject arrowPrefab, targetPrefab;
    public Vector2 archerPos, arrowArcherOffset;
    public float targetMinY;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval;

    private GameObject arrow, target;
    private Vector2 endTouchPos, swipe;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Spawner targetSpawner = new GameObject("TargetSpawner").AddComponent<Spawner>();
        targetSpawner.prefab = targetPrefab;
        targetSpawner.spawnInterval = 2f;
        targetSpawner.bounds = new Rect(-cameraWidth + .5f, -cameraHeight + .5f + targetMinY, cameraWidth * 2 - 1, cameraHeight * 2 - 1 - targetMinY); //

        RespawnArrow();
    }

    void Update()
    {
        if (arrow != null)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            swipe = arrow.transform.position - Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Moved)
            {
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (touch.phase == TouchPhase.Ended)
            {
#else
        {
            swipe = arrow.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0))
            {
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (Input.GetMouseButtonUp(0))
            {
#endif
                if (swipe.y > 0)
                {
                    ShootArrow(swipe);
                }
            }
        }
    }

    private void ShootArrow(Vector2 direction)
    {
        var force = swipe * forceMultiplier;
        var rigidBody = arrow.GetComponent<Rigidbody2D>();
        rigidBody.simulated = true;
        rigidBody.AddForce(force);
        arrow = null;
        StartCoroutine(Utils.DelayedAction(RespawnArrow, arrowRespawnInterval));
    }

    public void RespawnArrow()
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }

        arrow = Pool.Get(arrowPrefab);
        arrow.transform.position = archerPos + arrowArcherOffset;
    }
}
