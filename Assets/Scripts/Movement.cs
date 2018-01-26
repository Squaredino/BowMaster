using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public List<Vector2> waypoints;
    public float speed;
    public Ease ease;
    
    private Sequence sequence;
    private bool waypointsSet;

    void LateUpdate()
    {
        if (waypointsSet) return;
        waypointsSet = true;
        SetWaypoints();
    }

    void OnDisable()
    {
        waypointsSet = false;
        sequence.Kill();
    }

    public void Stop()
    {
        sequence.Kill();
    }

    private void SetWaypoints()
    {
        if (!waypoints.Any()) return;

        transform.position = waypoints[0];
        sequence = DOTween.Sequence();

        for (var i = 1; i < waypoints.Count; i++)
            sequence.Append(transform.DOMove(waypoints[i], Vector2.Distance(waypoints[i], waypoints[i - 1]) / speed).SetEase(ease));
        sequence.Append(transform.DOMove(waypoints[0], Vector2.Distance(waypoints[0], waypoints.Last()) / speed).SetEase(ease));

        sequence.SetLoops(-1);
        sequence.Play();

        waypoints.Clear();
    }
}