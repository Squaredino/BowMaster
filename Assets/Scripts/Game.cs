using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Game : MonoBehaviour
{
    public GameObject arrowPrefab, targetPrefab, aimAssistPrefab;
    public Vector2 archerPos, arrowArcherOffset;
    public float targetMinY;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval, targetRespawnInterval;
    public GameObject arrow;
    public Vector2 swipe;
    public Rect gameBounds;
    public Text scoreText;
    public int score, bullseyeStreak;
    public GameObject crown;
    public GameObject timerBarObj;
    public float timerTotalStart;
    public float timerHitBonus, timerBullseyeBonus;
    public Color cameraDefaultColor, cameraHitColor, cameraBullseyeColor;

    private Vector2 startTouchPos;
    private Spawner targetSpawner;
    private float swipeMagnitude;
    private TimerBar timerBar;
    private bool gameStarted = false;
    private bool isArrowFlying = false;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        gameBounds = new Rect(-cameraWidth, -cameraHeight, cameraWidth * 2, cameraHeight * 2);

        targetSpawner = new GameObject("TargetSpawner").AddComponent<Spawner>();
        targetSpawner.prefab = targetPrefab;
        targetSpawner.spawnStrategy = SpawnerStrategy.First;
        targetSpawner.bounds = new Rect(gameBounds.x + 1f, gameBounds.y + 1f + targetMinY, gameBounds.width - 2f, gameBounds.height - 2 - targetMinY); //
        targetSpawner.Spawn();

        Instantiate(aimAssistPrefab).GetComponent<AimAssist>();

        Camera.main.backgroundColor = cameraDefaultColor;

        timerBar = timerBarObj.GetComponent<TimerBar>();
        timerBar.ShowTimer();

        scoreText.text = PlayerPrefs.GetInt("Highscore").ToString();

        RespawnArrow();

        SceneManager.sceneLoaded += OnSceneLoaded;
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
        else
        {
            timerBar.SetTotalTime(timerTotalStart - score / 50f);
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
                    if (!gameStarted)
                    {
                        StartGame();
                    }
                    swipe = Vector2.zero;
                }
            }
        }
    }

    private void ShootArrow(Vector2 direction)
    {
        var force = direction * forceMultiplier;
        var arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.particleLevel = bullseyeStreak;
        arrowScript.Shoot(force);
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
        bullseyeStreak = isHit && isBullseye ? bullseyeStreak + 1 : 0;

        if (isHit)
        {
            SetSpawnerStrategy();
            StartCoroutine(Utils.DelayedAction(targetSpawner.Spawn, targetRespawnInterval));

            score += 1 + bullseyeStreak;
            scoreText.transform.DOPunchScale(Vector3.one * Mathf.Min(bullseyeStreak / 10f, 1), 0.3f);

            timerBar.AddTime(isBullseye ? timerBullseyeBonus : timerHitBonus);

            Camera.main.DOColor(isBullseye ? cameraBullseyeColor : cameraHitColor, 0.1f).OnComplete(() =>
                Camera.main.DOColor(cameraDefaultColor, 0.1f));
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
        crown.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).OnComplete(() => crown.SetActive(false));
        timerBar.StartTimer(timerTotalStart);
        gameStarted = true;
    }

    private void GameOver()
    {
        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        PlayerPrefs.SetInt("LastScore", score);

        SceneManager.LoadScene("GameOverScreen");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOverScreen")
        {
            GameObject.Find("RecordText").GetComponent<Text>().text = PlayerPrefs.GetInt("LastScore").ToString();
            GameObject.Find("HighscoreText").GetComponent<Text>().text = PlayerPrefs.GetInt("Highscore").ToString();
        }
    }
}
