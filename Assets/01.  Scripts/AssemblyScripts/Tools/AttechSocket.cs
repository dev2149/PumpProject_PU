using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;


public class AttechSocket : MonoBehaviour
{
    [SerializeField] private Tool.Type _BoltType;

    private Interactable interactable;

    private bool _IsTriggerDown;
    private bool _IsTriggerDownChk;

    private bool _IsTriggerUp;
    private bool _IsSocketDetech;

    public bool IsSocketDetech { get { return _IsSocketDetech; } }
    public Tool.Type BoltType { get { return _BoltType; } }

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(interactable.hoveringHand)
        {
            if(SteamVR_Input.GetStateDown("GrabPinch", interactable.hoveringHand.handType) && !_IsTriggerDownChk)
            {
                _IsTriggerDown = true;
                _IsTriggerDownChk = true;
            }

            if(SteamVR_Input.GetStateUp("GrabPinch", interactable.hoveringHand.handType) && _IsTriggerDownChk)
            {
                _IsTriggerUp = true;
                _IsSocketDetech = true;
            }
        }
        else
        {
            _IsTriggerDownChk = _IsTriggerDown = _IsTriggerUp = false;
        }
    }

    public void DataReset()
    {
        _IsSocketDetech = _IsTriggerDownChk = _IsTriggerDown = _IsTriggerUp = false;
    }
}