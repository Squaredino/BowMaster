﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public const float padding = 0.1f;
    public const float fadeInDuration = 1.0f, fadeOutDuration = 0.3f;

    public float displayDuration;
    
    public void Show(Vector3 position)
    {
        var game = GameObject.Find("Game").GetComponent<Game>();
        var render = GetComponent<Renderer>();

        position.x = Mathf.Min(position.x, game.gameBounds.width / 2f - render.bounds.extents.x - padding);
        position.x = Mathf.Max(position.x, game.gameBounds.x + render.bounds.extents.x + padding);
        position.y = Mathf.Min(position.y, game.gameBounds.height / 2f - render.bounds.extents.y - padding);
        position.y = Mathf.Max(position.y, game.gameBounds.y + render.bounds.extents.y + padding);

        transform.position = position;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, fadeInDuration).SetEase(Ease.OutElastic);
        StartCoroutine(Utils.DelayedAction(
            () => transform.DOScale(Vector3.zero, fadeOutDuration).OnComplete(
                () => Destroy(gameObject)), displayDuration));
    }
}