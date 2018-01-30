using UnityEngine;

/// <summary>
///     Скрипт подгоняющий размер камеры под указанную высоту в пикселях
///     (Пример: есть спрайт 600пкс в высоту. Размер экрана 300пкс. Камера будет видеть половину размера, поэтому
///     увеличиваем Size x2)
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraSizeToHeight : MonoBehaviour
{
    // Значение pixelPerUnit для данного спрайта
    public float PixelPerUnit = 100f;

    public float SizeMultiplier = 1f;

    public float SpriteHeight = 100f;

    private void Awake()
    {
        CalculateSize();
    }

    private void CalculateSize()
    {
        GetComponent<Camera>().orthographicSize = SizeMultiplier * SpriteHeight / (2f * PixelPerUnit);
    }
}