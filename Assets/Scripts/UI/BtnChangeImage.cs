using UnityEngine;
using UnityEngine.UI;

public class BtnChangeImage : MonoBehaviour
{
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;
    [SerializeField] private bool _isOn;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        if (!_isOn)
        {
            _image.sprite = _spriteOff;
        }
    }

    public void ChangeState(bool _flag)
    {
        _isOn = _flag;
        if (_isOn)
        {
            _image.sprite = _spriteOn;
        }
        else
        {
            _image.sprite = _spriteOff;
        }
    }
}
