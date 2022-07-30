using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PartName))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
public class AssemblyPart : MonoBehaviour
{
    [SerializeField] private AssemblyStep.PartType _PartType;
    [SerializeField] private GameObject[] _HandObject;
    [SerializeField] private GameObject _CubeObject;
    [SerializeField] private Vector3 _Direction;
    [SerializeField] private bool _IsAnimation;
    [SerializeField] private bool _IsStaticObject;
    [SerializeField] private bool _Foward;
    [SerializeField] private bool _Up;
    [SerializeField] private bool _Right;
    [SerializeField] private Tool.ToolType _ToolType;

    private AssemblyObject.AssemblyType _AssemblyType;

    private MeshRenderer _MeshRenderer;
    private Material[] _GhostMaterial;
    private Color[] _OriginColor;

    private AssemblyStep _AssemblyStep;
    private AssemblyTranslate _AssemblyTranslate;
    private DecompositionPartObject _DecompositionPartObject;
    private PartName _PartName;
    private Interactable _Interactable;
    private Throwable _Throwable;
    private Rigidbody _Rigidbody;
    private Collider[] _MeshColliders;

    private bool _IsAssembleCheck;
    private bool _IsAssembleEnd;
    private bool _IsDetechCheck;

    private bool _IsToolRunning;
    private bool _IsToolCheck;
    private bool _IsToolEnd;
    private bool _IsToolStart;

    private Vector3 _Position;
    private Transform _Rot;

    private Vector3 _OriginPosition;
    private Quaternion _OriginQuaternion;

    [SerializeField] private float DepthCount;
    [SerializeField] private float InsertSpeed = 5.0f;
    [SerializeField] private float RotationSpeed = 5.0f;

    private Coroutine _InsertRoutine;
    private Coroutine _InsertRotationRoutine;
    [SerializeField] private GameObject _HighLightObject;
    [SerializeField] private AudioSource _ScrewAudioClip;
    public bool IsAssembleCheck { get { return _IsAssembleCheck; } set { _IsAssembleCheck = value; } }
    public bool IsAssembleEnd { get { return _IsAssembleEnd; } }
    public bool IsToolEnd { get { return _IsToolEnd; } }
    public bool IsDetechCheck { get { return _IsDetechCheck; } }
    public Tool.ToolType ToolType { get { return _ToolType; } }
    public Vector3 Direction { get { return _Direction; } }

    public PartName PartName { get { return _PartName; } }
    public Vector3 Position { get { return _Position; } set { _Position = value; } }
    public Quaternion Rot { get { return _OriginQuaternion; }set { _OriginQuaternion = value; } }
    public AssemblyStep.PartType ObjectPartType { get { return _PartType; } }

    public Tool.Type BoltSize { get { return _PartName.states.ToolType; } }
    private void FixedUpdate()
    {
        //조립 그냥 하는중
        if (_IsAssembleEnd.Equals(false))
        {
            _Position = this.gameObject.transform.position;
        }
        //조립 후 볼트로 조립하는 경우
        else if (_IsToolRunning.Equals(true))
        {
            if (_IsToolEnd.Equals(true) && _IsToolCheck.Equals(false))
            {
                //for (int i = 0; i < _GhostMaterial.Length; i++)
                //{
                //    _GhostMaterial[i].SetColor("_Color", _OriginColor[i]);
                //    _MeshRenderer.materials = _GhostMaterial;
                //}

                if (_InsertRoutine != null)
                    StopCoroutine(_InsertRoutine);

                if (_InsertRotationRoutine != null)
                    StopCoroutine(_InsertRotationRoutine);

                _InsertRoutine = null;
                _InsertRotationRoutine = null;

                _IsToolCheck = true;
                _IsToolRunning = false;
                _CubeObject.SetActive(false);
                if (_ScrewAudioClip != null)
                {
                    _ScrewAudioClip.volume = Directory.Instance.soundManager._EffectSound.volume;
                    Directory.Instance.soundManager.StopSound(_ScrewAudioClip.clip);
                }
                if (_HighLightObject.activeSelf && _HighLightObject != null)
                {
                    _HighLightObject.SetActive(false);
                }
                if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Assembly))
                {
                    _AssemblyStep.OnAssemblyToolCheckMethod();

                }
                else if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Decomposition))
                {
                    // 각 부품들을 분해 했을때
                    DecompositionObject();
                }
            }
        }

        if (_DecompositionPartObject != null && _DecompositionPartObject.IsDetechObject.Equals(true) && _IsDetechCheck.Equals(false))
        {
            if (_DecompositionPartObject.DetechTypeData.Equals(DecompositionPartObject.DetechType.Fade))
            {
                //해당분해 하고자 하는 부품들이 다 분해 되었을때
                DecompositionObject();
                _IsDetechCheck = true;
            }
            else if (_DecompositionPartObject.DetechTypeData.Equals(DecompositionPartObject.DetechType.Animation))
            {
                _DecompositionPartObject.SetPartAnimation();
                _IsDetechCheck = true;
            }
        }
    }

    /// <summary>
    /// 바닥에 떨구면 돌아감
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            transform.position = _OriginPosition;
            transform.rotation = _OriginQuaternion;
        }
    }

    private void OnDetachedFromHand(Hand hand)
    {
        if (_IsAssembleEnd.Equals(false))
        {
            if (_IsAssembleCheck.Equals(true))
            {
                _AssemblyStep.OnAssemblyCheckMethod(_PartName.states.CurrentStep, hand.handType);
            }
        }

        if (_HandObject.Length > 0)
        {
            if (hand.handType.Equals(SteamVR_Input_Sources.LeftHand))
                _HandObject[0].SetActive(false);
            else if (hand.handType.Equals(SteamVR_Input_Sources.RightHand))
                _HandObject[1].SetActive(false);
        }

        _AssemblyStep.OnAssemblyObjectDetech(_PartName, hand.handType);

        //AssemblyScene.Instance.SetTag(hand.handType, null, string.Empty);
    }

    private void OnAttachedFromHand(Hand hand)
    {
        //Director.Instance.SoundManager.PlayEffectSound(SoundManager.EffectSoundIndex.ObjectGrap);

        _AssemblyStep.OnAssemblyObjectAttech(_PartName, hand.handType);

        if (_HandObject.Length > 0)
        {
            if (hand.handType.Equals(SteamVR_Input_Sources.LeftHand))
                _HandObject[0].SetActive(true);
            else if (hand.handType.Equals(SteamVR_Input_Sources.RightHand))
                _HandObject[1].SetActive(true);
        }

        //AssemblyScene.Instance.SetTag(hand.handType, this.transform, _PartName.states.ObjectName);
    }

    public void Init(AssemblyStep assemblyStep, AssemblyObject.AssemblyType assemblyType)
    {
        _AssemblyType = assemblyType;

        _OriginPosition = _Position = transform.position;
        _OriginQuaternion = transform.rotation;

        _AssemblyStep = assemblyStep;
        _IsAssembleEnd = false;

        _AssemblyTranslate = GetComponent<AssemblyTranslate>();
        _PartName = GetComponent<PartName>();

        if (_PartName != null)
            _PartName.Init();

        if (_AssemblyTranslate != null)
            _AssemblyTranslate.Init();

        if (_CubeObject != null)
        {
            _MeshRenderer = GetComponent<MeshRenderer>();
            _GhostMaterial = _MeshRenderer.materials;

            _OriginColor = new Color[_GhostMaterial.Length];

            for (int i = 0; i < _GhostMaterial.Length; i++)
            {
                _OriginColor[i] = _GhostMaterial[i].GetColor("_Color");
            }
        }

        if (!_PartType.Equals(AssemblyStep.PartType.Animation))
        {
            _Interactable = GetComponent<Interactable>();
            _Throwable = GetComponent<Throwable>();
            _Rigidbody = GetComponent<Rigidbody>();
            _MeshColliders = GetComponentsInChildren<Collider>();

            if (_Interactable != null)
            {
                _Interactable.onDetachedFromHand += OnDetachedFromHand;
                _Interactable.onAttachedToHand += OnAttachedFromHand;
            }
        }

        if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Assembly))
        {
            gameObject.SetActive(false);
        }
        else if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Decomposition))
        {
            if (_Rigidbody != null)
                _Rigidbody.isKinematic = true;

            _DecompositionPartObject = GetComponent<DecompositionPartObject>();

            AssembleEnd();
        }
    }
    public void AssembleStart()
    {
        AssemblyReadySetup();

        if (gameObject.activeSelf.Equals(false))
            gameObject.SetActive(true);

        transform.position = _OriginPosition;
        transform.rotation = _OriginQuaternion;
    }
    #region PUBLIC
    /// <summary>
    /// 조립 완료하려고 물리 제거할때 호출
    /// </summary>
    public void AssembleEnd()
    {
        if (_IsAssembleEnd.Equals(false))
        {
            AssemblyEndSetup();

            _IsAssembleEnd = true;
        }
    }
    /// <summary>
    /// 다음단계로 넘기기
    /// </summary>
    public void AssemblyNextSetup(AssemblyKeypoint assemblyKeypoint)
    {
        AssembleEnd();

        DirectionSetup(assemblyKeypoint);

        transform.position = assemblyKeypoint.Position;
        transform.rotation = assemblyKeypoint.transform.rotation;

        if (_PartType.Equals(AssemblyStep.PartType.Tool))
        {
            _IsToolRunning = false;

            if (_CubeObject != null)
            {
                _CubeObject.SetActive(false);
                _CubeObject.GetComponent<Collider>().enabled = false;
            }

            float temp = _PartName.states.BoltDepth * 0.0001f;
            Vector3 tempVector = transform.localPosition;

            tempVector = tempVector + (_Direction * temp);
            transform.localPosition = tempVector;
        }
        else if (_PartType.Equals(AssemblyStep.PartType.Animation))
        {
            if (_AssemblyTranslate != null)
            {
                _AssemblyTranslate.Set();
            }
        }
    }
    public void AssemblyPrev()
    {
        AssemblyReadySetup();

        if (gameObject.activeSelf.Equals(true))
            gameObject.SetActive(false);

        transform.position = _OriginPosition;
        transform.rotation = _OriginQuaternion;

        if (_PartType.Equals(AssemblyStep.PartType.Animation))
        {
            if (_AssemblyTranslate != null)
            {
                _AssemblyTranslate.ResetData();
            }
        }
    }

    /// <summary>
    /// 애니메이션으로 진행되는 경우 호출
    /// </summary>
    public void AssembleEndAnimation()
    {
        if (_PartType.Equals(AssemblyStep.PartType.Animation))
        {
            _IsAssembleCheck = true;

            if (_IsAssembleCheck.Equals(true))
            {
                _AssemblyStep.OnAssemblyCheckMethod(_PartName.states.CurrentStep, SteamVR_Input_Sources.Any);
            }

            if (_AssemblyTranslate != null)
            {
                _AssemblyTranslate.Set();
            }
        }
    }

    /// <summary>
    /// 나사 다풀면 오브젝트 원 위치
    /// </summary>
    public void DecompositionObject()
    {
        if (_IsStaticObject.Equals(false))
        {
            _IsDetechCheck = true;

            transform.position = _OriginPosition;
            transform.rotation = _OriginQuaternion;

            if (_ScrewAudioClip != null)
            {
                _ScrewAudioClip.volume = Directory.Instance.soundManager._EffectSound.volume;
                Directory.Instance.soundManager.StopSound(_ScrewAudioClip.clip);
            }
        }
    }

    #endregion

    #region Private

    /// <summary>
    /// 진행도초기화 and 시작할때 물리관련 켜줌
    /// </summary>
    private void AssemblyReadySetup()
    {
        _IsToolStart = _IsToolEnd = _IsToolRunning = _IsToolCheck = _IsAssembleCheck = _IsAssembleEnd = false;

        if (_PartType.Equals(AssemblyStep.PartType.Assemble))
        {
            _Interactable.enabled = true;
            _Throwable.enabled = true;
            _Rigidbody.isKinematic = false;

            _MeshColliders.ForEach((Collider mesh) => { mesh.enabled = true; });
        }
        else if (_PartType.Equals(AssemblyStep.PartType.Tool))
        {
            _Interactable.enabled = true;
            _Throwable.enabled = true;
            _Rigidbody.isKinematic = false;
            _MeshColliders.ForEach((Collider mesh) => { mesh.enabled = true; });

            DepthCount = Progress = 0.0f;

            if (_CubeObject != null)
            {
                _CubeObject.SetActive(false);
                _CubeObject.GetComponent<Collider>().enabled = false;
            }
        }
        if (_PartType.Equals(AssemblyStep.PartType.Animation))
        {
            if (_AssemblyTranslate != null)
            {
                _AssemblyTranslate.ResetData();
            }
        }
    }

    /// <summary>
    /// 건너뛰기 and 조립완료시 물리관련 꺼줌
    /// </summary>
    private void AssemblyEndSetup()
    {
        if (_PartType.Equals(AssemblyStep.PartType.Assemble))
        {
            _Interactable.enabled = false;
            _Throwable.enabled = false;
            _Rigidbody.isKinematic = true;

            _MeshColliders.ForEach((Collider mesh) => { mesh.enabled = false; });
        }
        else if (_PartType.Equals(AssemblyStep.PartType.Tool))
        {
            _Interactable.enabled = false;
            _Throwable.enabled = false;
            _Rigidbody.isKinematic = true;
            _MeshColliders.ForEach((Collider mesh) => { mesh.enabled = false; });
            _IsToolRunning = true;

            if (_CubeObject != null)
            {
                _CubeObject.SetActive(true);
                _CubeObject.GetComponent<Collider>().enabled = true;
            }
        }
    }

    #endregion

    #region 도구 사용하는 부분

    public float Progress;

    public void DirectionSetup(AssemblyKeypoint assemblyKeypoint)
    {
        if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Assembly))
            _Direction = -assemblyKeypoint.Direction;
        else if (_AssemblyType.Equals(AssemblyObject.AssemblyType.Decomposition))
            _Direction = assemblyKeypoint.Direction;
    }

    public void ToolTypeSetup(Tool.ToolType type)
    {
        _ToolType = type;
    }
    public void ActiveHightLight(bool _b)
    {
        if (_HighLightObject != null)
        {
            _HighLightObject.gameObject.SetActive(_b);
        }
    }
    public bool HighLightState()
    {
        bool b;
        if (_HighLightObject.gameObject.activeSelf)
        {

            b = false;
        }
        else
        {

            b = true;
        }
        return b;
    }
    public void InsertStart()
    {
        if (_IsToolStart.Equals(false))
        {
            _IsToolStart = true;

            _InsertRoutine = StartCoroutine(InsertRoutine());
            _InsertRotationRoutine = StartCoroutine(InsertRotationRoutine());
            for (int i = 0; i < _GhostMaterial.Length; i++)
            {
                _GhostMaterial[i].SetColor("_Color", Color.green);
                _MeshRenderer.materials = _GhostMaterial;
            }
            if (_ScrewAudioClip != null)
            {
                _ScrewAudioClip.volume = Directory.Instance.soundManager._EffectSound.volume;
                Directory.Instance.soundManager.PlaySound(_ScrewAudioClip.clip);
            }
        }
    }

    public void InsertStop()
    {
        if (_IsToolStart.Equals(true))
        {
            _IsToolStart = false;

            StopCoroutine(_InsertRoutine);
            StopCoroutine(_InsertRotationRoutine);
            _InsertRoutine = null;
            _InsertRotationRoutine = null;

            for (int i = 0; i < _GhostMaterial.Length; i++)
            {
                _GhostMaterial[i].SetColor("_Color", _OriginColor[i]);
                _MeshRenderer.materials = _GhostMaterial;
            }
            if (_ScrewAudioClip != null)
            {
                _ScrewAudioClip.volume = Directory.Instance.soundManager._EffectSound.volume;
                Directory.Instance.soundManager.StopSound(_ScrewAudioClip.clip);
            }
        }

    }

    IEnumerator InsertRoutine()
    {
        while (true)
        {
            DepthCount = DepthCount + InsertSpeed * Time.deltaTime;
            Progress = (DepthCount / _PartName.states.BoltDepth);

            float temp = DepthCount * 0.0001f;
            Vector3 tempVector = _Position;

            tempVector = tempVector + (_Direction * temp);
            transform.position = tempVector;

            if (DepthCount >= _PartName.states.BoltDepth)
            {
                _IsToolEnd = true;

                for (int i = 0; i < _GhostMaterial.Length; i++)
                {
                    _GhostMaterial[i].SetColor("_Color", _OriginColor[i]);
                    _MeshRenderer.materials = _GhostMaterial;
                }

                break;
            }
            yield return null;
        }
    }

    IEnumerator InsertRotationRoutine()
    {
        while (true)
        {
            if (_Foward)
                transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);
            else if (_Up)
                transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed);
            else if (_Right)
                transform.Rotate(Vector3.right * Time.deltaTime * RotationSpeed);

            yield return null;
        }
    }


    #endregion
}