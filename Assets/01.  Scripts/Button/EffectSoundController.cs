using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EffectSoundController : MonoBehaviour
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

    public bool _FlagEffectImage;
    [SerializeField] private AudioClip _ButtonClick;

    #endregion
    void Start()
    {
        _Source = GetComponent<Image>();
        _CurrentDisabled = Directory.Instance.soundManager._EffectDisabled;
        _CurrentHighlighted = Directory.Instance.soundManager._EffectHighlight;
        _Source.sprite = _CurrentDisabled;
        _FlagEffectImage = Directory.Instance.soundManager._flagEffectOnOff;
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
    public void SoundEffect()
    {
        SetChangeEffectIcon();
        Directory.Instance.soundManager.SetMuteSound("Effect");
    }
    void SetChangeEffectIcon()
    {
        _FlagEffectImage = !_FlagEffectImage;
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        if (_FlagEffectImage)
        {
            _CurrentHighlighted = _ChangeHightlighted;
            _CurrentDisabled = _ChangeDisabled;
            Directory.Instance.soundManager._EffectDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._EffectHighlight = _CurrentHighlighted;
        }
        else
        {
            _CurrentHighlighted = _Hightlighted;
            _CurrentDisabled = _Disabled;
            Directory.Instance.soundManager._EffectDisabled = _CurrentDisabled;
            Directory.Instance.soundManager._EffectHighlight = _CurrentHighlighted;
        }
    }
}
