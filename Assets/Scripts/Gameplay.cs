using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;
using TMPro;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour
{
    public const float gameAspect = 9f / 16f;
    public const float minScorePunch = .5f, maxScorePunch = 1.5f, scorePunchDuration = 0.3f;
    public const float highscoreScale = 1.6f, highscoreDuration = 0.25f;
    public const float crownFadeInDuration = 0.2f, crownFadeOutDuration = 0.5f;
    public const float minTargetScale = 0.6f;
    public const int positionsStoreCount = 10;

    [SerializeField] private PointsBallonManager _pointsBalloonManager;
    public GameObject arrowPrefab, targetPrefab, crossPrefab;
    public Vector2 arrowPos;
    public float minSwipeTime, maxSwipeTime;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval, targetRespawnInterval;
    public Rect gameBounds;
    [SerializeField] private TextMeshProUGUI scoreText, highscoreText;
    [SerializeField] private GameObject _hint;
    private int _gameplayCounter;
    public int bullseyeStreak, targetHits;
    private int score;
    public Image crown;
    public TimerBar timerBar;
    public float timerMaxTime, timerMinTime, timerReductionPerHit;
    public float targetAreaPadLeft, targetAreaPadTop, targetAreaPadRight, targetAreaPadBottom;
    public Spawner targetSpawner;
    public ParticleSystem fireworks;

    private GameObject arrow;
    private float swipeTime;
    private Queue<Vector2> touchPositions;
    private bool gameStarted, gamePaused;
    private bool isArrowFlying;
    private float forceCoef;
    private Cross cross;
    private Vector2 lastTouch;
    private bool _isReviveShowed;

    void Awake()
    {
        var cameraHeight = Camera.main.orthographicSize;
        var cameraWidth = cameraHeight * Mathf.Min(gameAspect, Camera.main.aspect);
        gameBounds = new Rect(-cameraWidth, -cameraHeight, cameraWidth * 2, cameraHeight * 2);

        touchPositions = new Queue<Vector2>();

        var daysInRow = PlayerPrefs.GetInt("PlayedDaysInRow", 1);
        var lastLogin = PlayerPrefs.GetInt("LastLoginTime", 0);
        var now = (int)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalHours;
        if (lastLogin == 0)
        {
            lastLogin = now;
            PlayerPrefs.SetInt("LastLoginTime", now);
        }
        var hoursSinceLastLogin = now - lastLogin;
        if (hoursSinceLastLogin >= 24)
        {
            if (hoursSinceLastLogin < 48)
                daysInRow++;
            else
                daysInRow = 1;
            PlayerPrefs.SetInt("LastLoginTime", now);
            PlayerPrefs.SetInt("PlayedDaysInRow", daysInRow);
        }

        GlobalEvents<OnChangeSkin>.Happened += OnChangeArrowSkin;
        GlobalEvents<OnChangeTargetSkin>.Happened += OnChangeTargetSkin;

        cross = Instantiate(crossPrefab).GetComponent<Cross>();
        cross.gameObject.SetActive(false);

        forceCoef = (maxForce - minForce) / (maxSwipeTime - minSwipeTime);

        _gameplayCounter = 0;

        GlobalEvents<OnGameAwake>.Call(new OnGameAwake { daysInRow = daysInRow });
    }

    void Start()
    {
        targetSpawner.bounds = new Rect(
            gameBounds.x + targetAreaPadLeft,
            gameBounds.y + targetAreaPadBottom,
            gameBounds.width - targetAreaPadLeft - targetAreaPadRight,
            gameBounds.height - targetAreaPadTop - targetAreaPadBottom);
        targetSpawner.scoreBounds = new Rect(
            scoreText.transform.position.x - 1.7f,
            scoreText.transform.position.y - 1.2f,
            3.4f,
            2.4f);
        targetSpawner.spawnStrategy = SpawnerStrategy.First;
        targetSpawner.Spawn();

        timerBar.ShowTimer();

        var highscore = PlayerPrefs.GetInt("Highscore");
        scoreText.text = highscore.ToString();
        if (highscore <= 0)
        {
            crown.DOFade(0f, 0f);
            scoreText.gameObject.SetActive(false);
        }

        RespawnArrow();

        _hint.SetActive(false);
        Invoke("ShowHintHand", 2f);

        GlobalEvents<OnGameLoaded>.Call(new OnGameLoaded());
    }

    void Update()
    {
        if (gameStarted && !gamePaused)
        {
            if (timerBar.IsTimerOver() && !isArrowFlying)
            {
                GameOver();
            }
        }
    }

    private void OnEnable()
    {
        GameInput.OnPointerDown += InputStart;
        GameInput.OnPointerUp += InputEnd;
        GameInput.OnPointerMove += InputMove;
        ReviveButton.OnReviveTimeEnded += OnReviveTimeEnd;
        ReviveButton.OnClickRevive += OnClickRevive;
    }
    
    private void OnClickRevive()
    {
        ResumeGame();
        timerBar.AddTime(1f);
    }

    private void OnReviveTimeEnd()
    {
        GameOver();
    }

    private void InputStart()
    {
        if (!gamePaused && arrow != null)
        {
            lastTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swipeTime = 0f;
        }
    }

    private void InputMove()
    {
        if (!gamePaused && arrow != null)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (position.y > lastTouch.y + 0.07f)
            {
                touchPositions.Enqueue((Vector2) position - lastTouch);
                if (touchPositions.Count > positionsStoreCount)
                {
                    touchPositions.Dequeue();
                }
            }

            swipeTime += Time.deltaTime;
            lastTouch = position;
        }
    }

    private void InputEnd()
    {
        if (!gamePaused && arrow != null)
        {
            InputMove();

            var swipe = Vector2.zero;
            foreach (var pos in touchPositions)
            {
                swipe += pos;
            }

            touchPositions.Clear();
            if (swipe.y <= 0.5f) return;

            arrow.transform.rotation = Quaternion.FromToRotation(Vector2.up, swipe);
            swipeTime = Mathf.Min(Mathf.Max(swipeTime, minSwipeTime), maxSwipeTime);
            var force = Mathf.Min(Mathf.Max(forceCoef * (maxSwipeTime - swipeTime), minForce), maxForce);
            ShootArrow(swipe, force * forceMultiplier);
        }
    }

    private void ShootArrow(Vector2 direction, float force)
    {
        var forceVector = direction.normalized * force;

        SetArrowParticles();
        arrow.GetComponent<Arrow>().Shoot(forceVector);

        isArrowFlying = true;
        arrow = null;

        StartCoroutine(Utils.DelayedAction(RespawnArrow, arrowRespawnInterval));
    }

    private void SetArrowParticles()
    {
        if (arrow != null)
        {
            arrow.GetComponent<Arrow>().SetParticleLevel(bullseyeStreak >= 2 ? 2 : bullseyeStreak);
        }
    }

    public void RespawnArrow()
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }

        arrow = Pool.Get(arrowPrefab);
        arrow.transform.position = arrowPos;
        SetArrowParticles();
    }

    public void TargetHit(bool isBullseye, Vector3 position)
    {
        targetHits++;
        bullseyeStreak = isBullseye ? bullseyeStreak + 1 : 0;
        score += 1 + bullseyeStreak;

        _pointsBalloonManager.Add(bullseyeStreak + 1, Camera.main.WorldToScreenPoint(position));
        scoreText.text = score.ToString();

        GlobalEvents<OnTargetHit>.Call(new OnTargetHit
        {
            score = score,
            totalScore = PlayerPrefs.GetInt("TotalScore", 0) + score,
            bullseyeStreak = bullseyeStreak,
            targetHits = targetHits,
            timerLeft = timerBar.TimeLeft,
            isTargetMoving = targetSpawner.spawnStrategy == SpawnerStrategy.SimpleMoving || targetSpawner.spawnStrategy == SpawnerStrategy.SimpleMovingVertical
        });

        scoreText.transform.localScale = Vector3.one;
        scoreText.transform.DOPunchScale(Vector3.one * Mathf.Min(minScorePunch + bullseyeStreak / 10f, maxScorePunch),
            scorePunchDuration);

        timerBar.StartTimer(Mathf.Max(timerMaxTime - timerReductionPerHit * targetHits, timerMinTime));

        SetArrowParticles();

        if (isBullseye)
        {
            GlobalEvents<OnIventPerfect>.Call(new OnIventPerfect());
        }
        else
        {
            GlobalEvents<OnIventSimple>.Call(new OnIventSimple());
        }

        SetSpawnerStrategy();
        StartCoroutine(Utils.DelayedAction(targetSpawner.Spawn, targetRespawnInterval));

        isArrowFlying = false;

        if (!gameStarted)
        {
            StartGame();
        }
    }

    public void TargetMiss(Vector3 position)
    {
        if (gameStarted) GameOver();
        cross.Show(position);
    }

    public string GetHitText()
    {
        return string.Format("+{0}", 1 + bullseyeStreak);
    }

    private void SetSpawnerStrategy()
    {
        var random = Random.value;
        if (random < 0.1f)
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
        }
        else if (random < 0.15f)
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMovingVertical;
        }
        else
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.Simple;
        }

        var minScale = Mathf.Max(minTargetScale, 1f - targetHits * 0.01f);
        targetSpawner.scale = Vector3.one * Random.Range(minScale, 1f);
    }

    private void StartGame()
    {
        if (!scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.DOFade(0f, 0f);
            scoreText.DOFade(1f, 0.5f);
        }
        crown.DOFade(0f, crownFadeOutDuration);
        timerBar.ContinueTimer();
        fireworks.Stop();
        cross.Hide();
        if (highscoreText.gameObject.activeSelf)
        {
            highscoreText.transform.DOScale(Vector3.zero, highscoreDuration)
                .OnComplete(() => highscoreText.gameObject.SetActive(false));
        }

        ++_gameplayCounter;
        CancelInvoke("ShowHintHand");
        if (_hint.activeSelf) _hint.SetActive(false);
        

        gameStarted = true;
        _isReviveShowed = false;

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Archer");
        GlobalEvents<OnStartGame>.Call(new OnStartGame { totalGames = PlayerPrefs.GetInt("TotalGamesPlayed", 0) });
    }

    private void PauseGame()
    {
        gamePaused = true;
        timerBar.PauseTimer();
        foreach (var target in targetSpawner.SpawnedObjects())
        {
            target.GetComponent<Movement>().TogglePause();
        }
    }

    private void ResumeGame()
    {
        gamePaused = false;
        timerBar.ContinueTimer();
        foreach (var target in targetSpawner.SpawnedObjects())
        {
            target.GetComponent<Movement>().TogglePause();
        }
    }

    private void ShowHintHand()
    {
        _hint.SetActive(true);
    }

    private void GameOver()
    {
        if (!_isReviveShowed)
        {
            _isReviveShowed = true;
            GlobalEvents<OnScreenReviveShow>.Call(new OnScreenReviveShow());
            PauseGame();
            return;
        }

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Archer", score);

        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) + score);

        CheckHighScore();

        score = 0;
        bullseyeStreak = 0;
        targetHits = 0;
        gameStarted = false;
        isArrowFlying = false;

        SetArrowParticles();
        crown.DOKill();
        crown.DOFade(1f, crownFadeInDuration);

        StopAllCoroutines();

        if (targetSpawner.spawnStrategy != SpawnerStrategy.First)
        {
            foreach (var target in targetSpawner.SpawnedObjects())
            {
                target.GetComponent<Target>().Stop();
                target.GetComponent<Target>().Despawn();
            }

            targetSpawner.spawnStrategy = SpawnerStrategy.First;
            targetSpawner.scale = Vector3.one;
            targetSpawner.Spawn();
        }

        timerBar.PauseTimer();
        StartCoroutine(Utils.DelayedAction(timerBar.ResetTimer, 0.3f));

        if (arrow == null)
        {
            RespawnArrow();
        }

        scoreText.text = PlayerPrefs.GetInt("Highscore").ToString();

        if (_gameplayCounter <= 3)
        {
            Invoke("ShowHintHand", 2f);
        }

        gamePaused = false;

        PlayerPrefs.SetInt("TotalGamesPlayed", PlayerPrefs.GetInt("TotalGamesPlayed", 0) + 1);

        PlayerPrefs.Save();
        GlobalEvents<OnGameOver>.Call(new OnGameOver());
    }

    private void CheckHighScore()
    {
        if (PlayerPrefs.GetInt("LastScore") > PlayerPrefs.GetInt("Highscore"))
        {
            fireworks.Play();

            highscoreText.transform.localScale = Vector3.zero;
            highscoreText.gameObject.SetActive(true);
            highscoreText.transform.DOScale(Vector3.one, highscoreDuration);

            scoreText.transform.DOScale(highscoreScale, highscoreDuration).SetLoops(6, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);

            PlayerPrefs.SetInt("Highscore", PlayerPrefs.GetInt("LastScore"));
            GlobalEvents<OnScreenRateShow>.Call(new OnScreenRateShow { IsBtnClick = false });
        }
    }

    private void OnChangeArrowSkin(OnChangeSkin obj)
    {
        foreach (var arrowObj in Pool.Objects(arrowPrefab))
        {
            arrowObj.GetComponent<Arrow>().LoadSkin(obj.Id);
        }
    }

    private void OnChangeTargetSkin(OnChangeTargetSkin obj)
    {
        foreach (var targetObj in Pool.Objects(targetPrefab))
        {
            TargetSkin[] arr = targetObj.GetComponentsInChildren<TargetSkin>();
            foreach (TargetSkin script in arr)
            {
                script.LoadSkin(obj.Id);
            }
        }
    }
}