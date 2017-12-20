using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Color normalColor;
    public Color limitColor;
    public float portionLimitColor;

    private float allTime = 10f;
    private float currentTime = 10f;
    private bool isActive;

    private Tweener tweener;

    private void Start()
    {
        GetComponent<Image>().color = normalColor;
        GetComponent<Image>().fillAmount = 1f;
        transform.parent.localScale = Vector3.zero;
    }

    public void ShowTimer()
    {
        if (tweener != null) tweener.Kill();
        transform.parent.DOScale(1.0f, 0.2f);
    }

    public void HideTimer()
    {
        if (tweener != null) tweener.Kill();
        isActive = false;
        transform.parent.DOScale(0.0f, 0.2f);
    }

    public void OnResetTimer()
    {
        isActive = false;
        GetComponent<Image>().fillAmount = 0f;
        GetComponent<Image>().color = normalColor;
    }

    public void ChangeSize()
    {
        if (tweener == null || !tweener.IsPlaying())
            tweener = transform.parent.DOScale(1.2f, 0.2f).OnComplete(() =>
                tweener = transform.parent.DOScale(1f, 0.4f));
    }

    public void StartTimer(float time)
    {
        allTime = time;
        currentTime = allTime;
        isActive = true;
    }

    public void SetTotalTime(float time)
    {
        currentTime *= time / allTime;
        allTime = time;
    }

    public void PauseTimer()
    {
        isActive = false;
    }

    public void ContinueTimer()
    {
        isActive = true;
    }

    public void AddTime(float percent)
    {
        currentTime = Mathf.Min(currentTime + percent * allTime, allTime);
        isActive = true;
    }

    public bool IsTimerOver()
    {
        return currentTime <= 0f;
    }

    void Update()
    {
        if (isActive)
        {
            currentTime -= Time.deltaTime;
            float res = currentTime / allTime;
            GetComponent<Image>().color = normalColor;
            if (res <= portionLimitColor)
            {
                GetComponent<Image>().color = limitColor;
                ChangeSize();
            }
            GetComponent<Image>().fillAmount = res;

            if (currentTime <= 0f)
            {
                GetComponent<Image>().color = normalColor;
                isActive = false;
            }
        }
    }
}
