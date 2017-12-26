using System.Collections;
using UnityEngine;

public class Profiler : MonoBehaviour
{
    private float accum; // FPS accumulated over the interval
    public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
    private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    private int frames; // Frames drawn over the interval
    public float frequency = 0.5F; // The update frequency of the fps
    public int nbDecimal = 1; // How many decimal do you want to display
    private string sFPS = ""; // The fps formatted into a string.
    public Rect startRect = new Rect(5, 5, 75, 30); // The rect the window is initially displayed at.
    private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.
    public bool updateColor = true; // Do you want the color to change if the FPS gets low

    private void Start()
    {
        StartCoroutine(FPS());
    }

    private void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }

    private IEnumerator FPS()
    {
        // Infinite loop executed every "frenquency" secondes.
        while (true)
        {
            // Update the FPS
            var fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));

            //Update the color
            color = fps >= 30 ? Color.green : (fps > 10 ? Color.red : Color.yellow);

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSeconds(frequency);
        }
    }

    private void OnGUI()
    {
        // Copy the default label skin, change the color and the alignement
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = updateColor ? color : Color.white;
        startRect = GUI.Window(0, startRect, DoMyWindow, "");
    }

    private void DoMyWindow(int windowID)
    {
        var label = sFPS + " FPS";

        GUI.Label(new Rect(0, 0, startRect.width, startRect.height), label, style);
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}