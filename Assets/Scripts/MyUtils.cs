using System;
using System.Collections;
using UnityEngine;

public static class MyUtils
{
    public static IEnumerator DelayedAction(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public static Vector3 RestrictVector(Vector3 vector, float minMagnitude, float maxMagnitude)
    {
        float magnitude = vector.magnitude;
        if (magnitude > 0)
        {
            if (magnitude < minMagnitude)
            {
                vector *= minMagnitude / magnitude;
            }
            if (magnitude > maxMagnitude)
            {
                vector *= maxMagnitude / magnitude;
            }
        }
        return vector;
    }
    
    public static bool VersionGraterThan(float version)
    {
        var str = SystemInfo.operatingSystem;
        string tmp = String.Empty;
        bool point = false;
        for (int i = 0; i < str.Length; i++)
        {
            if (Char.IsDigit(str[i]))
            {
                tmp += str[i];
                continue;
            }
            if (str[i] == '.')
            {
                if (!point)
                {
                    tmp += str[i];
                    point = true;
                }
                else break;
            }
        }
        if (Convert.ToSingle(tmp) >= version) return true;
        return false;
    }
}
