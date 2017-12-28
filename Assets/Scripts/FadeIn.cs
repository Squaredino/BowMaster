using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeInDuration;

    private void OnBecameVisible()
    {
        if (!DOTween.IsTweening(transform))
        {
            var scale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(scale, fadeInDuration);
        }
    }
}
