using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Game : MonoBehaviour
{
    public const float gameAspect = .5f;
    public const float minScorePunch = .5f, maxScorePunch = 1.5f, scorePunchDuration = 0.3f;

    public GameObject arrowPrefab, targetPrefab, aimAssistPrefab;
    public Vector2 archerPos, arrowArcherOffset;
    public float minSwipeTime, maxSwipeTime;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval, targetRespawnInterval;
    public GameObject arrow;
    public Vector2 swipe;
    public Rect gameBounds;
    public Text scoreText;
    public int score, bullseyeStreak;
    public GameObject crown;
    public GameObject timerBarObj;
    public float timerStartTime, timerMinTime;
    public float timeReductionPerHit;
    public Color cameraDefaultColor, cameraHitColor, cameraBullseyeColor;
    public bool reverseControls;
    public float targetAreaPadLeft, targetAreaPadTop, targetAreaPadRight, targetAreaPadBottom;

    private Vector2 startTouchPos;
    private Spawner targetSpawner;
    private float swipeTime;
    private TimerBar timerBar;
    private int targetHits = 0;
    private bool gameStarted = false;
    private bool isArrowFlying = false;
    private float forceCoef;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Mathf.Min(gameAspect, Camera.main.aspect);
        gameBounds = new Rect(-cameraWidth, -cameraHeight, cameraWidth * 2, cameraHeight * 2);

        targetSpawner = new GameObject("TargetSpawner").AddComponent<Spawner>();
        targetSpawner.prefab = targetPrefab;
        targetSpawner.spawnStrategy = SpawnerStrategy.First;
        targetSpawner.bounds = new Rect(
            gameBounds.x + targetAreaPadLeft,
            gameBounds.y + targetAreaPadBottom,
            gameBounds.width - targetAreaPadLeft - targetAreaPadRight,
            gameBounds.height - targetAreaPadTop - targetAreaPadBottom);
        targetSpawner.Spawn();

        //Instantiate(aimAssistPrefab).GetComponent<AimAssist>();

        Camera.main.backgroundColor = cameraDefaultColor;

        forceCoef = (maxForce - minForce) / (maxSwipeTime - minSwipeTime);

        timerBar = timerBarObj.GetComponent<TimerBar>();
        timerBar.ShowTimer();

        scoreText.text = PlayerPrefs.GetInt("Highscore").ToString();

        RespawnArrow();
    }

    void Update()
    {
        if (arrow != null)
        {
            HandleInput();
        }

        if (gameStarted)
        {
            scoreText.text = score.ToString();
        }

        if (timerBar.IsTimerOver() && !isArrowFlying)
        {
            GameOver();
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
                swipeTime = 0f;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
                swipe = reverseControls ? -swipe : swipe;
                swipeTime += Time.deltaTime;
                arrow.transform.rotation = Quaternion.FromToRotation(Vector2.up, swipe);
            }
            if (touch.phase == TouchPhase.Ended)
            {
#else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                swipeTime = 0f;
            }
            if (Input.GetMouseButton(0))
            {
                swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                swipe = reverseControls ? -swipe : swipe;
                swipeTime += Time.deltaTime;
                arrow.transform.rotation = Quaternion.FromToRotation(Vector2.up, swipe);
            }
            if (Input.GetMouseButtonUp(0))
            {
#endif
                swipe = startTouchPos - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                swipe = reverseControls ? -swipe : swipe;
                swipeTime += Time.deltaTime;
                arrow.transform.rotation = Quaternion.FromToRotation(Vector2.up, swipe);
                if (swipe.y > 0)
                {
                    swipeTime = Mathf.Min(Mathf.Max(swipeTime, minSwipeTime), maxSwipeTime);
                    var force = Mathf.Min(Mathf.Max(forceCoef * (maxSwipeTime - swipeTime), minForce), maxForce);
                    ShootArrow(swipe, force * forceMultiplier); 

                    if (!gameStarted)
                    {
                        StartGame();
                    }
                }
            }
        }
    }

    private void ShootArrow(Vector2 direction, float force)
    {
        var forceVector = direction.normalized * force;

        var arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.particleLevel = bullseyeStreak;
        arrowScript.Shoot(forceVector);

        isArrowFlying = true;
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

    public void TargetHit(bool isHit, bool isBullseye = false)
    {
        if (isHit)
        {
            targetHits++;

            bullseyeStreak = isBullseye ? bullseyeStreak + 1 : 0;
            score += 1 + bullseyeStreak;
            scoreText.transform.DOPunchScale(Vector3.one * Mathf.Min(minScorePunch + bullseyeStreak / 10f, maxScorePunch), scorePunchDuration);
            
            timerBar.StartTimer(Mathf.Max(timerStartTime - timeReductionPerHit * targetHits, timerMinTime));

            Camera.main.DOColor(isBullseye ? cameraBullseyeColor : cameraHitColor, 0.1f).OnComplete(() =>
                Camera.main.DOColor(cameraDefaultColor, 0.1f));

            SetSpawnerStrategy();
            StartCoroutine(Utils.DelayedAction(targetSpawner.Spawn, targetRespawnInterval));
        }
        else
        {
            GameOver();
        }

        isArrowFlying = false;
    }

    private void SetSpawnerStrategy()
    {
        if (UnityEngine.Random.value < 0.1)
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
        }
        else
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.Simple;
        }
    }

    private void StartGame()
    {
        crown.GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() => crown.SetActive(false));
        timerBar.StartTimer(timerStartTime);
        gameStarted = true;
    }

    private void GameOver()
    {
        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        PlayerPrefs.SetInt("LastScore", score);

        SceneManager.LoadScene("Game"); //("GameOverScreen");
    }
}
