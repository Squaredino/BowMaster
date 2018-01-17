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
    
    public float startWidth, endWidth;
    public Color startColor, endColor;
    
    private LineRenderer lineRenderer;
    
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "cross";
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
    
    void Update()
    {

    }
}
