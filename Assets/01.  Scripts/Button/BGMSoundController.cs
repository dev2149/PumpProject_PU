using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BGMSoundController : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent OnClick;
    #region HighlightGroup
    [SerializeField] private Sprite _Hightlighted;
    [SerializeField] private Sprite _Disabled;
    [SerializeField] private Sprite _ChangeHightlighted;
    [SerializeField] private Sprite _ChangeDisabled;
    [SerializeField] private Sprite _CurrentHighlighted;
    [SerializeField] private Sprite _CurrentDisabled;
    private Image _Source;

    public bool _FlagBGMImage;
    [SerializeField] private AudioClip _ButtonClick;
    #endregion
    void Start()
    {
        _Source = GetComponent<Image>();
        _CurrentDisabled = Directory.Instance.soundManager._BGMDisabled;
        _CurrentHighlighted = Directory.Instance.soundManager._BGMHighlight;
        _Source.sprite = _CurrentDisabled;
        _FlagBGMImage = Directory.Instance.soundManager._flagBGMOnOff;
    }
    public void PointerClick()
    {
        OnClick.Invoke();
    }
    public void PointerInside()
    {
        SetSprite(_CurrentHighlighted);
    }
    public void PointerOutside()
    {
        SetSprite(_CurrentDisabled);
    }
    void SetSprite(Sprite _sprite)
    {
        _Source.overrideSprite = _sprite;
    }
    public void SoundEnvironment()
    {
        SetChangeBGMIcon();
        Directory.Instance.soundManager.SetMuteSound("BGM");
    }
    void SetChangeBGMIcon()
    {
        _FlagBGMImage = !_FlagBGMImage;
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        if (_FlagBGMImage)
        {
            _CurrentHighlighted = _ChangeHightlighted;
            _CurrentDisabled = _ChangeDisabled;
            Directory.Instance.soundManager._BGMDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._BGMHighlight = _CurrentHighlighted;
        }
        else
        {
            _CurrentHighlighted = _Hightlighted;
            _CurrentDisabled = _Disabled;
            Directory.Instance.soundManager._BGMDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._BGMHighlight = _CurrentHighlighted;
        }
    }
}
