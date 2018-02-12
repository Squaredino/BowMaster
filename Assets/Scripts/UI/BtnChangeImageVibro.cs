using UnityEngine;
using UnityEngine.UI;

public class BtnChangeImageVibro : MonoBehaviour
{
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        if (!GameSettings.IsVibro)
        {
            _image.sprite = _spriteOff;
        }
    }

    public void ChooseSprite()
    {
        if (GameSettings.IsVibro)
        {
            _image.sprite = _spriteOn;
        }
        else
        {
            _image.sprite = _spriteOff;
        }
    }
}
