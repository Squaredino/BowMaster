using System.Collections;
using PrefsEditor;
using UnityEngine;

public class AppSeconds : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SaveSeconds());
    }

    private IEnumerator SaveSeconds()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            SaveSeconds(GetSeconds() + 1);
        }
    }

    private void SaveSeconds(int seconds)
    {
        seconds = Mathf.Clamp(seconds, 0, int.MaxValue);
        PlayerPrefs.SetInt("GameTotalTime", seconds);
    }

    public static int GetSeconds()
    {
        var seconds = 0;
        seconds = PlayerPrefs.GetInt("GameTotalTime", 0);
        seconds = Mathf.Clamp(seconds, 0, int.MaxValue);
        return seconds;
    }
}