using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int score;
    public GameObject timerBarObj;
    public float timerTotalStart;
    public float timerHitBonus, timerBullseyeBonus;

    private AimAssist aimAssist;
    private Vector2 startTouchPos;
    private Spawner targetSpawner;
    private float swipeMagnitude;
    private int bullseyeStreak;
    private TimerBar timerBar;

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

        timerBar = timerBarObj.GetComponent<TimerBar>();
        timerBar.ShowTimer();
        timerBar.StartTimer(timerTotalStart);
        timerBar.TimerOver += GameOver;

        RespawnArrow();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (arrow != null)
        {
            HandleInput();
        }

        scoreText.text = score.ToString();

        timerBar.SetTotalTime(timerTotalStart - Mathf.Sqrt(score / 10f));
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
        arrow.GetComponent<Arrow>().particleLevel = bullseyeStreak;
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
    }

    public void TargetHit(bool isHit, bool isBullseye = false)
    {
        bullseyeStreak = isHit && isBullseye ? bullseyeStreak + 1 : 0;

        if (isHit)
        {
            SetSpawnerStrategy();
            StartCoroutine(Utils.DelayedAction(targetSpawner.Spawn, targetRespawnInterval));

            score += 1 + bullseyeStreak;

            timerBar.AddTime(isBullseye ? timerBullseyeBonus : timerHitBonus);
        }
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
            GameObject.Find("HighscoreText").GetComponent<Text>().text = string.Format("BEST: {0}", PlayerPrefs.GetInt("Highscore"));
        }
    }
}
