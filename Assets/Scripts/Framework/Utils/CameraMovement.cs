using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float DampTime = 0.15f;
    [HideInInspector] public bool IsMoveToPosition;
    [HideInInspector] public bool IsMovingToTarget;
    public Transform Target;
    [HideInInspector] public Vector3 TargetPosition;

    // Update is called once per frame
    private void Update()
    {
        if (Target && IsMovingToTarget)
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, DampTime) +
                                 new Vector3(0.0f, 0f, -10f);
        }
        else if (IsMoveToPosition)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.01f);
            if (Mathf.Abs(transform.position.x - TargetPosition.x) < 0.02f)
            {
                transform.position = TargetPosition;
                IsMoveToPosition = false;
            }
        }
    }

    public void StartMoving()
    {
        IsMovingToTarget = true;
    }

    public void StopMoving()
    {
        IsMovingToTarget = false;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, -10f);
    }
}