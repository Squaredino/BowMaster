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
    public Rect gameBounds;

    private AimAssist aimAssist;
    private Vector2 startTouchPos;
    private Spawner targetSpawner;
    private float swipeMagnitude;
    private int bullseyeStreak;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        gameBounds = new Rect(-cameraWidth, -cameraHeight, cameraWidth * 2, cameraHeight * 2);

        targetSpawner = new GameObject("TargetSpawner").AddComponent<Spawner>();
        targetSpawner.prefab = targetPrefab;
        targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
        targetSpawner.bounds = new Rect(gameBounds.x + .5f, gameBounds.y + .5f + targetMinY, gameBounds.width - 1, gameBounds.height - 1 - targetMinY); //
        targetSpawner.Spawn();

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
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
                swipe = Utils.RestrictVector(swipe, minForce, maxForce);
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
            if (Input.GetMouseButton(0))
            {
                swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                swipe = Utils.RestrictVector(swipe, minForce, maxForce);
                arrow.transform.rotation = Quaternion.FromToRotation(Vector2.up, swipe);
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
        arrow.GetComponent<Arrow>().Shoot(force);
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

    public void TargetHit(bool isHit, bool isBullseye = false)
    {
        bullseyeStreak = isHit && isBullseye ? bullseyeStreak + 1 : 0;

        if (isHit)
        {
            if (UnityEngine.Random.value < 0.1)
            {
                targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
            }
            else
            {
                targetSpawner.spawnStrategy = SpawnerStrategy.Simple;
            }
            targetSpawner.Spawn();
        }
    }
}
