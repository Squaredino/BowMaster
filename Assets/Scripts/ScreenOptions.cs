using UnityEngine;

[ExecuteInEditMode]
public class ScreenOptions : MonoBehaviour
{
    [SerializeField] private int _frameRate = 60;
    [SerializeField] private float _gameHeight = 9f;
    [SerializeField] private float _gameWidth = 16f;
    [SerializeField] private bool _isChangeCameraSize = true;
    [SerializeField] private bool _isNeverSleep = true;
    [SerializeField] private float GameAspect { get; set; }

    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }
    public static float ScreenAspect { get; private set; }

    public static float ScreenWidthAspect { get; private set; }
    public static float ScreenHeightAspect { get; private set; }

    private void Awake()
    {
        SetScreenSettings();
        if (_isChangeCameraSize) SetScreenSize();
    }

#if UNITY_EDITOR
    private void OnRenderObject()
    {
        if (Application.isPlaying) return;
        SetScreenSettings();
        if (_isChangeCameraSize) SetScreenSize();
    }
#endif

    private void SetScreenSettings()
    {
        if (_isNeverSleep) Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = _frameRate;
    }

    private void SetScreenSize()
    {
        GameAspect = _gameWidth / _gameHeight;
        ScreenAspect = Camera.main.aspect;

        if (ScreenAspect > GameAspect)
        {
            ScreenWidth = _gameHeight * ScreenAspect;
            ScreenHeight = _gameHeight;
        }
        else
        {
            ScreenWidth = _gameWidth;
            ScreenHeight = _gameWidth / ScreenAspect;
        }

        ScreenWidthAspect = ScreenWidth / _gameWidth;
        ScreenHeightAspect = ScreenHeight / _gameHeight;

        Camera.main.orthographicSize = ScreenHeight / 2f;
    }
}