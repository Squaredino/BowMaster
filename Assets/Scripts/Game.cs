using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject arrowPrefab, targetPrefab, aimAssistPrefab;
    public Vector2 archerPos, arrowArcherOffset;
    public float targetMinY;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval;
    public GameObject arrow;
    public Vector2 swipe;

    private AimAssist aimAssist;
    private Vector2 endTouchPos;
    private int bullseyeStreak;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Spawner targetSpawner = new GameObject("TargetSpawner").AddComponent<Spawner>();
        targetSpawner.prefab = targetPrefab;
        targetSpawner.spawnInterval = 2f;
        targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
        targetSpawner.bounds = new Rect(-cameraWidth + .5f, -cameraHeight + .5f + targetMinY, cameraWidth * 2 - 1, cameraHeight * 2 - 1 - targetMinY); //

        aimAssist = Instantiate(aimAssistPrefab).GetComponent<AimAssist>();

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
            if (touch.phase == TouchPhase.Moved)
            {
                swipe = arrow.transform.position - Camera.main.ScreenToWorldPoint(touch.position);
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (touch.phase == TouchPhase.Ended)
            {
#else
        {
            if (Input.GetMouseButton(0))
            {
                swipe = arrow.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                arrow.transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), swipe);
            }
            if (Input.GetMouseButtonUp(0))
            {
#endif
                if (swipe.y > 0)
                {
                    ShootArrow(swipe);
                    swipe = Vector2.zero;
                }
            }
        }
    }

    private void ShootArrow(Vector2 direction)
    {
        var force = direction * forceMultiplier;
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
        arrow.GetComponent<Arrow>().particleLevel = bullseyeStreak;
    }

    public void TargetHit(bool isBullseye = false)
    {
        bullseyeStreak = isBullseye ? bullseyeStreak + 1 : 0;
    }
}
