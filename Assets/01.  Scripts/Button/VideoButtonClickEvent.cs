using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VideoButtonClickEvent : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent OnClick;
    public Sprite _Hightlighted;
    public Sprite _Disabled;
    public Sprite _Selected;
    const float _DELAYSELECTBUTTONTIME = 0.1f;
    private Image _Source;
    private BoxCollider _MyCollider;
    void Start()
    {
        _Source = GetComponent<Image>();
        _MyCollider = GetComponent<BoxCollider>();
    }
    public void SetImagesInit()
    {
        SetSprite(_Disabled);
    }
    #region InOutEvent
    public void PointerClick()
    {
        StartCoroutine(ClickHighLight());
    }
    public void PointerInside()
    {
        SetSprite(_Hightlighted);
    }
    public void PointerOutside()
    {
        SetSprite(_Disabled);
    }
    #endregion
    public void SetSprite(Sprite _sprite)
    {
        _Source.overrideSprite = _sprite;
    }
    IEnumerator ClickHighLight()
    {
        SetSprite(_Selected);
        yield return new WaitForSeconds(_DELAYSELECTBUTTONTIME);
        _MyCollider.enabled = false;
        yield return new WaitForSeconds(_DELAYSELECTBUTTONTIME);
        _MyCollider.enabled = true;
        OnClick.Invoke();
    }
}