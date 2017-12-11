using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
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
}
