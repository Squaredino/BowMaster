using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Game : MonoBehaviour
{
    public const float gameAspect = 9f / 16f;
    public const float minScorePunch = .5f, maxScorePunch = 1.5f, scorePunchDuration = 0.3f;
    public const float bgColorInDuration = 0.05f, bgColorOutDuration = 0.8f;
    public const float highscoreScale = 1.6f, highscoreDuration = 0.25f;
    public const float cameraShakeDuration = 0.2f, cameraShakeStrength = 0.1f, cameraShakeVibratio = 30f;
    public const float crownFadeInDuration = 0.2f, crownFadeOutDuration = 0.5f;
    public const float minTargetScale = 0.6f;
    public const long vibrateDuration = 100;
    public const int positionsStoreCount = 10;

    public GameObject arrowPrefab, targetPrefab, crossPrefab;
    public Vector2 arrowPos, fireworksPos;
    public float minSwipeTime, maxSwipeTime;
    public float minForce, maxForce, forceMultiplier;
    public float arrowRespawnInterval, targetRespawnInterval;
    public Rect gameBounds;
    public Text scoreText, highscoreText;
    public int score, bullseyeStreak, targetHits;
    public Image crown, background;
    public TimerBar timerBar;
    public float timerMaxTime, timerMinTime, timerReductionPerHit;
    public Color bgDefaultColor, bgHitColor, bgBullseyeColor;
    public float targetAreaPadLeft, targetAreaPadTop, targetAreaPadRight, targetAreaPadBottom;
    public string[] bullseyeText;
    public Spawner targetSpawner;
    public ParticleSystem fireworks;

    private GameObject arrow;
    private float swipeTime;
    private Queue<Vector2> touchPositions;
    private bool gameStarted;
    private bool isArrowFlying;
    private float forceCoef;
    private Cross cross;
    private Vector2 lastTouch;

    void Start()
    {
        var cameraHeight = Camera.main.orthographicSize;
        var cameraWidth = cameraHeight * Mathf.Min(gameAspect, Camera.main.aspect);
        gameBounds = new Rect(-cameraWidth, -cameraHeight, cameraWidth * 2, cameraHeight * 2);

        targetSpawner.bounds = new Rect(
            gameBounds.x + targetAreaPadLeft,
            gameBounds.y + targetAreaPadBottom,
            gameBounds.width - targetAreaPadLeft - targetAreaPadRight,
            gameBounds.height - targetAreaPadTop - targetAreaPadBottom);
        targetSpawner.spawnStrategy = SpawnerStrategy.First;
        targetSpawner.Spawn();

        touchPositions = new Queue<Vector2>();

        cross = Instantiate(crossPrefab).GetComponent<Cross>();
        cross.gameObject.SetActive(false);
        
        background.color = bgDefaultColor;

        forceCoef = (maxForce - minForce) / (maxSwipeTime - minSwipeTime);

        timerBar.ShowTimer();

        CheckHighScore();

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
            GameReset();
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
                InputStart(Camera.main.ScreenToWorldPoint(touch.position));
            }
            if (touch.phase == TouchPhase.Moved)
            {
                InputMove(Camera.main.ScreenToWorldPoint(touch.position));
            }
            if (touch.phase == TouchPhase.Ended)
            {
                InputEnd(Camera.main.ScreenToWorldPoint(touch.position));
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            InputStart(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
            InputMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonUp(0))
        {
            InputEnd(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
#endif
    }

    private void InputStart(Vector3 position)
    {
        lastTouch = position;
        swipeTime = 0f;
    }

    private void InputMove(Vector3 position)
    {
        if (position.y > lastTouch.y + 0.07f)
        {
            touchPositions.Enqueue((Vector2)position - lastTouch);
            if (touchPositions.Count > positionsStoreCount)
            {
                touchPositions.Dequeue();
            }
        }

        swipeTime += Time.deltaTime;
        lastTouch = position;
    }

    private void InputEnd(Vector3 position)
    {
        InputMove(position);

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

        if (!gameStarted)
        {
            StartGame();
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

    public void TargetHit(bool isBullseye = false)
    {
        targetHits++;
        bullseyeStreak = isBullseye ? bullseyeStreak + 1 : 0;
        score += 1 + bullseyeStreak;

        scoreText.transform.localScale = Vector3.one;
        scoreText.transform.DOPunchScale(Vector3.one * Mathf.Min(minScorePunch + bullseyeStreak / 10f, maxScorePunch),
            scorePunchDuration);

        timerBar.StartTimer(Mathf.Max(timerMaxTime - timerReductionPerHit * targetHits, timerMinTime));

        if (isBullseye)
        {
            Camera.main.DOShakePosition(cameraShakeDuration, cameraShakeStrength, (int) cameraShakeVibratio);
            if (bullseyeStreak >= 3)
            {
                Vibration.Vibrate(vibrateDuration);
            }
        }

        SetArrowParticles();

        if (isBullseye)
        {
            background.DOKill();
            background.DOColor(bullseyeStreak > 2 ? bgBullseyeColor : bgHitColor, bgColorInDuration)
                .OnComplete(() => background.DOColor(bgDefaultColor, bgColorOutDuration));
        }

        SetSpawnerStrategy();
        StartCoroutine(Utils.DelayedAction(targetSpawner.Spawn, targetRespawnInterval));

        isArrowFlying = false;
    }

    public void TargetMiss(Vector3 position)
    {
        GameReset();
        cross.Show(position);
    }

    public string GetBullseyeText()
    {
        if (bullseyeStreak < 3)
        {
            return bullseyeText[bullseyeStreak];
        }

        return bullseyeText[3 + bullseyeStreak % (bullseyeText.Length - 3)];
    }

    private void SetSpawnerStrategy()
    {
        var random = UnityEngine.Random.value;
        if (random < 0.1)
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMoving;
        }
        else if (random < 0.15)
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.SimpleMovingVertical;
        }
        else
        {
            targetSpawner.spawnStrategy = SpawnerStrategy.Simple;
        }

        var minScale = Mathf.Max(minTargetScale, 1f - targetHits * 0.01f);
        targetSpawner.scale = Vector3.one * UnityEngine.Random.Range(minScale, 1f);
    }

    private void StartGame()
    {
        crown.DOFade(0f, crownFadeOutDuration);
        timerBar.ContinueTimer();
        fireworks.Stop();
        cross.Hide();
        if (highscoreText.gameObject.activeSelf)
        {
            highscoreText.transform.DOScale(Vector3.zero, highscoreDuration)
                .OnComplete(() => highscoreText.gameObject.SetActive(false));
        }

        gameStarted = true;
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
        }
    }

    private void GameReset()
    {
        PlayerPrefs.SetInt("LastScore", score);
        CheckHighScore();

        score = 0;
        bullseyeStreak = 0;
        targetHits = 0;
        gameStarted = false;
        isArrowFlying = false;

        SetArrowParticles();
        targetSpawner.DespawnAll();
        targetSpawner.spawnStrategy = SpawnerStrategy.First;
        targetSpawner.Spawn();
        timerBar.StartTimer(timerMaxTime);
        timerBar.PauseTimer();
        crown.DOKill();
        crown.DOFade(1f, crownFadeInDuration);
        StopAllCoroutines();
        if (arrow == null)
        {
            RespawnArrow();
        }

        scoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }
}