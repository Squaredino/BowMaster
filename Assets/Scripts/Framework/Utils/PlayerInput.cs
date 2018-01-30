using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool _isPointerPressed;

    private void Start()
    {
        transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public bool IsPointerPressed()
    {
        return _isPointerPressed;
    }
    
    public void CancelPointerPress()
    {
        _isPointerPressed = false;
    }

    public Vector2 GetPointerPosition()
    {
        Vector2 screenPosition;

        if (IsMobilePlatform())
        {
            if (Input.touchCount == 0) return Vector2.zero;
            screenPosition = Input.touches[0].position;
        }
        else
        {
            screenPosition = Input.mousePosition;
        }
        
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    public void OnPointerDown()
    {
        _isPointerPressed = true;
    }

    public void OnPointerUp()
    {
        _isPointerPressed = false;
    }
    
    bool IsMobilePlatform()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer ||
               Application.platform == RuntimePlatform.Android;
    }
}
