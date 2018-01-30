using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float LifeTime = 3f;

    private void Awake()
    {
        Destroy(gameObject, LifeTime);
    }
}