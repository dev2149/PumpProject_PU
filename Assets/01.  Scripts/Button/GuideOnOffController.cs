using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideOnOffController : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent OnClick;
    [SerializeField] private Sprite _GuideOn;
    [SerializeField] private Sprite _GuideOff;
    private Sprite _CurrentImage;
    private Image _Source;
    private bool _FlagImage;
    void Start()
    {
        _Source = GetComponent<Image>();
        //_FlagImage = Directory.Instance.soundManager._flagGuideOnOff;
        //_CurrentImage = Directory.Instance.soundManager._GuideSprite;
        SetSprite(_CurrentImage);
    }
    public void PointerClick()
    {
        OnClick.Invoke();
    }
    public void SetChangeIcon()
    {
        _FlagImage = !_FlagImage;
        if (_FlagImage)
        {
            _CurrentImage = _GuideOn;
            //Directory.Instance.soundManager._flagGuideOnOff = _FlagImage;
            //Directory.Instance.soundManager._GuideSprite = _CurrentImage;
        }
        else
        {
            _CurrentImage = _GuideOff;
            //Directory.Instance.soundManager._flagGuideOnOff = _FlagImage;
            //Directory.Instance.soundManager._GuideSprite = _CurrentImage;
        }
        SetSprite(_CurrentImage);
    }
    void SetSprite(Sprite _sprite)
    {
        _Source.overrideSprite = _sprite;
    }
}
