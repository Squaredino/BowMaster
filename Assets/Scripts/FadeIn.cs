using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeInDuration;

    private void OnBecameVisible()
    {
        var scale = transform.localScale;
        if (!DOTween.IsTweening(transform))
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(scale, fadeInDuration);
        }
    }
}
