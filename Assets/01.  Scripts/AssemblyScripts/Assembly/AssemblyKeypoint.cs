using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


[RequireComponent(typeof(PartName))]
public class AssemblyKeypoint : MonoBehaviour
{
    [SerializeField] private AssemblyStep.PartType _PartType;
    [SerializeField] private Tool.ToolType _ToolType;
    [SerializeField] private Vector3 _Direction;

    private PartName _PartName;
    private MeshRenderer[] _MeshRenderer;
    private Material[][] _GhostMaterial;

    private Color _BaseColor;
    private Color _ChangeColor;

    private bool _IsAssemblyCheck;
    private bool _IsAssembleEnd;

    private Vector3 _Position;

    private SteamVR_Input_Sources _CheckHand = SteamVR_Input_Sources.Any;

    public bool IsAssembleEnd { get { return _IsAssembleEnd; } }
    public bool IsAssemblyCheck { get { return _IsAssemblyCheck; } set { _IsAssemblyCheck = value; } }

    public Vector3 Position { get { return _Position; } }
    public PartName PartName { get { return _PartName; } }
    public Vector3 Direction { get { return _Direction; } }

    public Tool.ToolType ToolType { get { return _ToolType; } }
    public SteamVR_Input_Sources CheckHand { get { return _CheckHand; } set { _CheckHand = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(AssemblyObject.AssemblyType assemblyType)
    {
        if(assemblyType.Equals(AssemblyObject.AssemblyType.Assembly))
        {
            _PartName = GetComponent<PartName>();
            _MeshRenderer = GetComponentsInChildren<MeshRenderer>();

            _PartName.Init();

            if (_PartType.Equals(AssemblyStep.PartType.Tool))
            {
                float temp = _PartName.states.BoltDepth * 0.0001f;
                Vector3 tempVector = transform.localPosition;

                tempVector = tempVector + (_Direction * temp);
                transform.localPosition = tempVector;
            }

            if (_MeshRenderer != null)
            {
                _GhostMaterial = new Material[_MeshRenderer.Length][];

                for (int i = 0; i < _MeshRenderer.Length; i++)
                {
                    _GhostMaterial[i] = _MeshRenderer[i].materials;
                }
            }

            _Position = this.gameObject.transform.position;

            _BaseColor = _GhostMaterial[0][0].GetColor("_Color");
            _ChangeColor = Color.green;
            _ChangeColor.a = 1 * 0.69f;
        }
        else
        {
            AssembleEnd();
        }
    }

    /// <summary>
    /// 조립 시작할때 호출
    /// </summary>
    public void AssemblyStart()
    {
        _IsAssemblyCheck =_IsAssembleEnd = false;
        _CheckHand = SteamVR_Input_Sources.Any;

        if (_MeshRenderer != null)
            SetColor(false);

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 조립 완료시 호출
    /// </summary>
    public void AssembleEnd()
    {
        _IsAssembleEnd = true;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 현재단계 진행 건너뛰기
    /// </summary>
    public void AssemblyNextSetup()
    {
        AssembleEnd();
    }

    /// <summary>
    /// 현재단계 진행도 초기화
    /// </summary>
    public void AssemblyPrev()
    {
        _IsAssembleEnd = false;
        _CheckHand = SteamVR_Input_Sources.Any;

        gameObject.SetActive(true);
    }

    public void SetColor(bool b)
    {
        if (b == true /*&& _IsAssemblyCheck.Equals(false)*/)
        {
            _IsAssemblyCheck = b;

            for(int j = 0; j < _MeshRenderer.Length; j++)
            {
                for (int i = 0; i < _GhostMaterial[j].Length; i++)
                {
                    _GhostMaterial[j][i].SetColor("_Color", _ChangeColor);
                }

                _MeshRenderer[j].materials = _GhostMaterial[j];
            }

        }
        else if (b == false /*&& _IsAssemblyCheck.Equals(true)*/)
        {
            _IsAssemblyCheck = b;

            for (int j = 0; j < _MeshRenderer.Length; j++)
            {
                for (int i = 0; i < _GhostMaterial[j].Length; i++)
                {
                    _GhostMaterial[j][i].SetColor("_Color", _BaseColor);
                }

                _MeshRenderer[j].materials = _GhostMaterial[j];
            }
        }
    }
}
