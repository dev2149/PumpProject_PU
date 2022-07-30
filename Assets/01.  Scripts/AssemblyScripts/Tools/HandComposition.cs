using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class HandComposition : Tool
{
    [SerializeField] GameObject m_HandHint;
    [SerializeField] Text m_HintMsg;
    private void Start()
    {
        m_HandHint = GameObject.Find("Hand_Hint").gameObject.transform.Find("Hint_trackpad_Start").gameObject;
        m_HintMsg = GameObject.Find("Hand_Hint").gameObject.transform.Find("Hint_trackpad_Start").gameObject.transform.
            Find("CanvasOffset").GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            _AttachObject = other.GetComponentInParent<AssemblyPart>();

            _IsAssemblyPartColliderIn = true;

            Tool.Type BoltSize = _AttachObject.BoltSize;
            Tool.ToolType ToolType = _AttachObject.ToolType;
            Debug.Log(BoltSize);
            Debug.Log(_Type);
            //수공구 전동공구 나누지 않음
            if (ToolType.Equals(Tool.ToolType.None))
            {
                if (!BoltSize.Equals(_Type))
                {
                    //m_HandHint.gameObject.SetActive(true);
                    //m_HintMsg.text = "적절하지 않은 \n 공구 입니다.";
                    //_InsertCheck = false;
                }
                else
                {
                    //m_HandHint.gameObject.SetActive(false);
                    //_ToolCheck.SetPartName(null, "");
                    _InsertCheck = true;
                }
            }
            else
            {
                if (!BoltSize.Equals(_Type) || !ToolType.Equals(_ToolType))
                {
                    //_ToolCheck.SetPartName(this.transform, "적절하지 않은 공구입니다");
                    _InsertCheck = false;
                    Debug.Log("c");

                }
                else
                {
                    //_ToolCheck.SetPartName(null, "");
                    _InsertCheck = true;
                    Debug.Log("d");

                }
            }
        }
    }
    protected override void HandAttachedUpdate(Hand hand)
    {
        base.HandAttachedUpdate(hand);

        if (_AttachObject != null && _IsAssemblyPartColliderIn.Equals(true))
        {
            Debug.Log("check");

            //들어가기 시작하는 부분
            if (_IsTriggerDown && _IsTriggerDownChk.Equals(false) && _InsertCheck.Equals(true))
            {
                Debug.Log("in");
                _IsTriggerDownChk = true;
                _IsHaptic = true;

                _HapticRoutine = StartCoroutine(HapticRoutine());
                _AttachObject.InsertStart();
            }

            //
            if (_IsTriggerUp && _IsHaptic)
            {
                _IsHaptic = false;
                _IsTriggerDownChk = false;
                if (_HapticRoutine != null)
                {
                    StopCoroutine(_HapticRoutine);
                    _HapticRoutine = null;
                }

                _AttachObject.InsertStop();
            }

            //
            if (_AttachObject.IsToolEnd.Equals(true) && _IsHaptic.Equals(true))
            {
                if (_HapticRoutine != null)
                {
                    StopCoroutine(_HapticRoutine);
                    _HapticRoutine = null;
                    _AttachObject = null;

                    _IsTriggerDownChk = _IsHaptic = false;
                }
            }
        }
        else
        {
            if (_HapticRoutine != null)
            {
                StopCoroutine(_HapticRoutine);
                _AttachObject.InsertStop();

                _AttachObject = null;
                _HapticRoutine = null;
                _AttachObject = null;

                _IsTriggerDownChk = _IsHaptic = false;
            }
        }
    }
    protected override void OnAttachedToHand(Hand attachedHand)
    {
        base.OnAttachedToHand(attachedHand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);
    }

    protected override void OnHandFocusLost(Hand hand)
    {
        base.OnHandFocusLost(hand);
    }
}
