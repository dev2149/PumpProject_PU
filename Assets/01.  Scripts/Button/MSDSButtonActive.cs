using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MSDSButtonActive : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent OnClick;
    [SerializeField] private Sprite _FirstImage;
    [SerializeField] private Sprite _Hightlighted;
    [SerializeField] private Sprite _Disabled;
    [SerializeField] private Sprite _Selected;
    const float _DELAYSELECTBUTTONTIME = 0.1f;
    private Image _Source;
    public BoxCollider _MyCollider;
    void Start()
    {
        _Source = GetComponent<Image>();
        _MyCollider = GetComponent<BoxCollider>();
        _MyCollider.enabled = false;
            ;
        SetSprite(_FirstImage);
    }
    public void PointerClick()
    {
        StartCoroutine(ClickHighLight());
    }
    #region InOutEvent
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
