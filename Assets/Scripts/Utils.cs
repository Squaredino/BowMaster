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
}
