using UnityEngine;

public class Line : MonoBehaviour
{
    public Vector2 StartPoint
    {
        get { return lineRenderer.GetPosition(0); }
        set { lineRenderer.SetPosition(0, value); }
    }

    public Vector2 EndPoint
    {
        get { return lineRenderer.GetPosition(1); }
        set { lineRenderer.SetPosition(1, value); }
    }

    public bool useWorldSpace;
    public float startWidth, endWidth;
    public Color startColor, endColor;
    
    private LineRenderer lineRenderer;
    private bool isDrawing;
    private float movingSpeed;
    private Vector2 movingEndPoint;
    private float t;
    
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "cross";
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.useWorldSpace = useWorldSpace;

        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
    
    void Update()
    {
        if (isDrawing && movingSpeed != 0)
        {
            EndPoint = Vector2.Lerp(EndPoint, movingEndPoint, movingSpeed > 0 ? t : 1 + t);
            t += Time.deltaTime * movingSpeed;

            if (Mathf.Abs(t) >= 1)
            {
                isDrawing = false;
                t = 0.0f;
            }
        }
    }

    public void Move(Vector2 endPoint, float speed)
    {
        if (isDrawing) return;

        movingEndPoint = endPoint;
        movingSpeed = speed;
        isDrawing = true;
    }
}
