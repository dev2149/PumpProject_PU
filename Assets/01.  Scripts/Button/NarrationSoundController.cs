using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NarrationSoundController : MonoBehaviour
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

    public bool _FlagNarrationImage;
    [SerializeField] private AudioClip _ButtonClick;
    #endregion
    void Start()
    {
        _Source = GetComponent<Image>();
        _CurrentDisabled = Directory.Instance.soundManager._NarrationDisabled;
        _CurrentHighlighted = Directory.Instance.soundManager._NarrationHighLight;
        _Source.sprite = _CurrentDisabled;
        _FlagNarrationImage = Directory.Instance.soundManager._flagNarrationOnOff;
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
    public void SoundNarration()
    {
        SetChangeBGMIcon();
        Directory.Instance.soundManager.SetMuteSound("Narration");
    }
    void SetChangeBGMIcon()
    {
        _FlagNarrationImage = !_FlagNarrationImage;
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        if (_FlagNarrationImage)
        {
            _CurrentHighlighted = _ChangeHightlighted;
            _CurrentDisabled = _ChangeDisabled;
            Directory.Instance.soundManager._NarrationDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._NarrationHighLight = _CurrentHighlighted;
        }
        else
        {
            _CurrentHighlighted = _Hightlighted;
            _CurrentDisabled = _Disabled;
            Directory.Instance.soundManager._NarrationDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._NarrationHighLight = _CurrentHighlighted;
        }
    }
}
