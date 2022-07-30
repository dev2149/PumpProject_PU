using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ElecImpact : Tool
{
    public enum SocketSize
    {
        None = -1,
        LongSocket13,
        LongSocket17,
        LongSocket19,
        LongSocket22,
        ShortSocket13,
        ShortSocket17,
        ShortSocket19,
        ShortSocket22,
        LenchSocketLenchM8,
        LenchSocketLenchM10,
        LenchSocketLenchM12,
        UniJoint
    }

    [SerializeField] private AssemblyProgressUI _AssemblyProgressUI;

    [SerializeField] private Transform _Drill;
    [SerializeField] private Transform _UnivJoint;

    [SerializeField] AttechSocket[] _SocketList;
    [SerializeField] private float RotationSpeed = 5.0f;

    [Header("-Sounds-")]
    [SerializeField] private AudioSource _ImpactSound;
    private Coroutine _AudioFade;

    private Transform _SelectTarget;
    private Vector3 _TargetDir;

    private AttechSocket _Socket;
    private Coroutine _RotationRoutine;

    private ImpactSocket _InSocket;
    private bool _IsSocketIn;
    private SocketSize _InSocketSize;

    private bool _IsAttechObjectChk;
    //private NameTag _ToolCheck;

    // Start is called before the first frame update
    void Start()
    {
        //_ToolCheck = GameObject.Find("ToolPopUp").GetComponent<NameTag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            _AttachObject = other.GetComponentInParent<AssemblyPart>();
            _AssemblyProgressUI.SetUse(true);

            _IsAssemblyPartColliderIn = true;

            Tool.Type BoltSize = _AttachObject.BoltSize;
            Tool.ToolType ToolType = _AttachObject.ToolType;

            //수공구 전동공구 나누지 않음
            if (ToolType.Equals(Tool.ToolType.None))
            {
                if (!BoltSize.Equals(_Socket.BoltType))
                {
                    _InsertCheck = false;
                    //_ToolCheck.SetPartName(this.transform, "적절하지 않은 공구입니다");
                }
                else
                {
                    //_ToolCheck.SetPartName(null, "");
                    _InsertCheck = true;
                }

            }
            else
            {
                //볼트 사이즈 다르거나 공구 타입이 다를경우
                if (!BoltSize.Equals(_Socket.BoltType) || !ToolType.Equals(_ToolType))
                {
                    //_ToolCheck.SetPartName(this.transform, "적절하지 않은 공구입니다");
                    _InsertCheck = false;
                }
                else
                {
                    //_ToolCheck.SetPartName(null, "");
                    _InsertCheck = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            //_AttachObject = null;
            _AssemblyProgressUI.SetUse(false);

            _IsAssemblyPartColliderIn = false;
            _IsAttechObjectChk = false;

            //_ToolCheck.SetPartName(null, "");
        }
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        base.HandAttachedUpdate(hand);

        //트리거 누를때
        if (_IsTriggerDown && !_IsTriggerDownChk)
        {
            _IsTriggerDownChk = true;
            _IsHaptic = true;

            _ImpactSound.Play();

            _RotationRoutine = StartCoroutine(RotationRoutine());
            _HapticRoutine = StartCoroutine(HapticRoutine2());
        }

        //트리거 땔때
        if (_IsTriggerUp && _IsTriggerDownChk)
        {
            StopImpact();

            //만약 볼트 들어가는 중이였다면
            if (_IsAttechObjectChk && _IsAssemblyPartColliderIn && _AttachObject != null)
            {
                _AttachObject.InsertStop();
                _IsAttechObjectChk = false;
            }
        }

        if (_Socket != null)
        {
            //볼트 들어가게
            if (_IsHaptic && !_IsAttechObjectChk && _IsAssemblyPartColliderIn && _AttachObject != null && _InsertCheck.Equals(true))
            {
                _AssemblyProgressUI.SetUse(true);
                _AttachObject.InsertStart();
                _IsAttechObjectChk = true;
            }

            //만약 들어가는 중에 땐다면
            if (_IsAttechObjectChk && !_IsAssemblyPartColliderIn && _AttachObject != null)
            {
                _AssemblyProgressUI.SetUse(false);

                _AttachObject.InsertStop();
                _AttachObject = null;
                _IsAttechObjectChk = false;
            }

            if (_AttachObject != null && _AttachObject.IsToolEnd.Equals(true))
            {
                //Director.Instance.SoundManager.PlayEffectSound(SoundManager.EffectSoundIndex.Complete);

                _AssemblyProgressUI.SetUse(false);

                _AttachObject = null;
                _IsAttechObjectChk = false;

                StopImpact();
            }

            //소켓 때는거
            if (_Socket.IsSocketDetech)
            {
                _Socket.DataReset();
                _InSocket.ReturnSocket();

                _Socket.gameObject.SetActive(false);
                _Socket = null;
                _InSocket = null;
                _IsSocketIn = true;
                _InSocketSize = SocketSize.None;
            }

            //프로그레스 업데이트
            if (_AttachObject != null && _AttachObject.IsToolEnd.Equals(false))
            {
                _AssemblyProgressUI.Progress = _AttachObject.Progress;
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

        if(_Socket != null)
        {
            _Socket.DataReset();
            _InSocket.ReturnSocket();

            _Socket.gameObject.SetActive(false);
            _Socket = null;
            _InSocket = null;
            _IsSocketIn = true;
            _InSocketSize = SocketSize.None;
        }
    }

    protected override void OnHandFocusLost(Hand hand)
    {
        base.OnHandFocusLost(hand);
    }

    public void AttachSocket(ImpactSocket socket)
    {
        if (_Socket != null)
        {
            _Socket.DataReset();
            _Socket.gameObject.SetActive(false);
            _InSocket.ReturnSocket();
        }

        _InSocket = socket;
        _InSocketSize = _InSocket.SocketSize;
        _SocketList[(int)_InSocketSize].gameObject.SetActive(true);
        _Socket = _SocketList[(int)_InSocketSize];

        if (_InSocket.SocketSize != SocketSize.UniJoint)
        {
            _SelectTarget = _Drill;
            _TargetDir = Vector3.right;
        }
        else
        {
            _SelectTarget = _UnivJoint;
            _TargetDir = Vector3.forward;
        }

        _InSocket.MountSocket();
    }

    IEnumerator RotationRoutine()
    {
        while (true)
        {
            _SelectTarget.transform.Rotate(_TargetDir * Time.deltaTime * RotationSpeed);

            yield return null;
        }
    }

    IEnumerator StopSound()
    {
        float time = 1.0f;

        while (time >= 0.0f)
        {
            time -= Time.deltaTime;

            _ImpactSound.volume = Mathf.Clamp(time, 0, 1);

            yield return null;
        }
        _ImpactSound.Stop();

        _AudioFade = null;

        yield break;
    }

    private void StopImpact()
    {
        _IsTriggerDown = _IsHaptic = _IsTriggerDownChk = false;

        if (_RotationRoutine != null)
        {
            StopCoroutine(_RotationRoutine);
            _RotationRoutine = null;
        }

        if (_HapticRoutine != null)
        {
            StopCoroutine(_HapticRoutine);
            _HapticRoutine = null;
        }

        _ImpactSound.Stop();
    }
}