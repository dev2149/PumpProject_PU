using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ImpactSocket : MonoBehaviour
{
    [SerializeField] ElecImpact.SocketSize _SocketSize;
    [SerializeField] Collider _SocketCollider;
    [SerializeField] Interactable _Interactable;
    public ElecImpact.SocketSize SocketSize { get { return _SocketSize; } }

    private Vector3 _OriginPosition;
    private Quaternion _OriginQuaternion;
    private bool _IsMount;
    

    // Start is called before the first frame update
    void Start()
    {
        _Interactable = GetComponent<Interactable>();

        _OriginPosition = this.transform.position;
        _OriginQuaternion = this.transform.rotation;
    }

    public void OnPickUp()
    {
        _SocketCollider.isTrigger = true;

        if(_Interactable != null && _Interactable.attachedToHand.otherHand.currentAttachedObject != null)
        {
            var impact = _Interactable.attachedToHand.otherHand.currentAttachedObject.GetComponent<ElecImpact>();
            impact.AttachSocket(this);
        }
    }

    public void OnDetechFromHand()
    {
        if(_IsMount.Equals(true))
        {
            gameObject.SetActive(false);
        }

        _SocketCollider.isTrigger = false;

        this.transform.position = _OriginPosition;
        this.transform.rotation = _OriginQuaternion;
    }

    /// <summary>
    /// 소켓 장착
    /// </summary>
    public void MountSocket()
    {
        _IsMount = true;

        if (_Interactable != null)
        {
            gameObject.transform.position = Vector3.zero;
        }
    }

    /// <summary>
    /// 복귀
    /// </summary>
    public void ReturnSocket()
    {
        _IsMount = false;

        this.transform.position = _OriginPosition;
        this.transform.rotation = _OriginQuaternion;

        gameObject.SetActive(true);
    }
}
