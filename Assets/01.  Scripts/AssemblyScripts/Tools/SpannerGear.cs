using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class SpannerGear : Tool
{
    [SerializeField] GameObject m_HandHint;
    [SerializeField] Text m_HintMsg;
    [SerializeField] AudioSource _GrapSound;
    void Start()
    {
        m_HandHint = GameObject.Find("Hand_Hint").gameObject.transform.Find("Hint_trackpad_Start").gameObject;
        m_HintMsg = GameObject.Find("Hand_Hint").gameObject.transform.Find("Hint_trackpad_Start").gameObject.transform.
            Find("CanvasOffset").GetComponentInChildren<Text>();
        EffectSoundStart();
    }
    void EffectSoundStart()
    {
        _GrapSound.volume = Directory.Instance.soundManager._EffectSound.volume;
        Directory.Instance.soundManager.PlaySound(_GrapSound.clip);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            _AttachObject = other.GetComponentInParent<AssemblyPart>();

            _IsAssemblyPartColliderIn = true;

            Tool.Type BoltSize = _AttachObject.BoltSize;
            Tool.ToolType ToolType = _AttachObject.ToolType;

            //수공구 전동공구 나누지 않음
            if (ToolType.Equals(Tool.ToolType.None))
            {
                if (!BoltSize.Equals(_Type))
                {
                    if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
                    {
                        m_HandHint.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                        m_HandHint.gameObject.SetActive(true);
                        m_HintMsg.text = "적절하지 않은 \n 공구입니다";
                    }
                    _InsertCheck = false;
                }
                else
                {
                    m_HandHint.gameObject.SetActive(false);
                    _AttachObject.ActiveHightLight(false);

                    _InsertCheck = true;
                }
            }
            else
            {
                if (!BoltSize.Equals(_Type) || !ToolType.Equals(_ToolType))
                {
                    if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
                    {
                        m_HandHint.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                        m_HandHint.gameObject.SetActive(true);
                        m_HintMsg.text = "적절하지 않은 \n 공구입니다";
                    }
                    _InsertCheck = false;

                }
                else
                {
                    m_HandHint.gameObject.SetActive(false);
                    _AttachObject.ActiveHightLight(false);

                    _InsertCheck = true;

                }
            }
        }
        // 아웃라인 체크
        if (other.CompareTag("OutLineCheck"))
        {

            _AttachObject = other.GetComponentInParent<AssemblyPart>();

            _IsAssemblyPartColliderIn = true;

            Tool.Type BoltSize = _AttachObject.BoltSize;
            Tool.ToolType ToolType = _AttachObject.ToolType;

            if (ToolType.Equals(Tool.ToolType.None))
            {
                if (!BoltSize.Equals(_Type))
                {
                    m_HandHint.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                    m_HandHint.gameObject.SetActive(true);
                    m_HintMsg.text = "적절하지 않은 \n 공구입니다";
                    _InsertCheck = false;
                    CheckOutLine(other , BoltSize);
                }
                else
                {
                    m_HandHint.gameObject.SetActive(false);
                    CheckOutLine(other , BoltSize);

                    //_AttachObject.ActiveHightLight(false);
                    //_InsertCheck = true;
                }
            }
            else
            {
                if (!BoltSize.Equals(_Type) || !ToolType.Equals(_ToolType))
                {
                    m_HandHint.gameObject.SetActive(true);
                    m_HintMsg.text = "적절하지 않은 \n 공구입니다";
                    _InsertCheck = false;
                    CheckOutLine(other , BoltSize);
                }
                else
                {
                    m_HandHint.gameObject.SetActive(false);
                    CheckOutLine(other , BoltSize);

                    //_AttachObject.ActiveHightLight(false);
                    //_InsertCheck = true;

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
            {
                _AttachObject.ActiveHightLight(true);
            }
             m_HandHint.gameObject.SetActive(false);
            _IsAssemblyPartColliderIn = false;
            other.GetComponent<AssemblyPart>().InsertStop();
        }
        if (other.CompareTag("OutLineCheck"))
        {
            m_HandHint.gameObject.SetActive(false);
            _IsAssemblyPartColliderIn = false;
            other.GetComponent<AssemblyPart>().InsertStop();
        }
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        base.HandAttachedUpdate(hand);

        if (_AttachObject != null && _IsAssemblyPartColliderIn.Equals(true))
        {
            //들어가기 시작하는 부분
            if (_IsTriggerDown && _IsTriggerDownChk.Equals(false) && _InsertCheck.Equals(true))
            {
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
                    // 볼트 조립 개당 완료
                    _AttachObject.InsertStop();

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
                _AttachObject.InsertStop();

            }
        }
    }
    private void CheckOutLine(Collider other , Tool.Type _type) // 공구 타입과 해당 볼트가 같을 경우 동작
    {
        if (_AttachObject.HighLightState())
        {
            m_HandHint.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            m_HandHint.gameObject.SetActive(true);
            m_HintMsg.text = "표시된 볼트 부터 \n 체결해 주세요";
            _InsertCheck = false;
        }
        else
        {
            if (_type.Equals(_Type))
            {
                //m_HandHint.gameObject.SetActive(false);
                //_AttachObject.ActiveHightLight(false);
                _InsertCheck = true;
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