using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AssemblyStep : MonoBehaviour
{
    public enum PartType
    {
        Assemble,
        Animation,
        Tool
    }

    public class AttechPartCheckData
    {
        public SteamVR_Input_Sources _HandSource;
        public PartName.States States;
        public bool _NomalAssembly;
        public bool _NotMatcingAssembly;
        public int _NotMactingKeypoint;

        public AttechPartCheckData()
        {
            States = new PartName.States();

            _NotMatcingAssembly = _NomalAssembly = false;
            _NotMactingKeypoint = -1;
        }
    }

    public delegate void AssemblyCheckMethod(int value, SteamVR_Input_Sources handType);
    public delegate void AssemblyObjectAttech(PartName part, SteamVR_Input_Sources handType);
    public delegate void AssemblyObjectDetech(PartName part, SteamVR_Input_Sources handType);
    public delegate void AssemblyToolCheckMethod();

    private event AssemblyCheckMethod _AssemblyCheckMethod;
    private event AssemblyObjectAttech _AssemblyObjectAttech;
    private event AssemblyObjectDetech _AssemblyObjectDetech;
    private event AssemblyToolCheckMethod _AssemblyToolCheckMethod;

    [SerializeField] private List<AssemblyPart> _AssemblyPart;
    [SerializeField] private List<AssemblyKeypoint> _AssemblyKeypoint;
    [SerializeField] private AssemblyStep _PrevStep;

    [SerializeField] private PartName _LeftHandPart;
    private AttechPartCheckData _LeftAttechPartCheckData;
    [SerializeField]private PartName _RightHandPart;
    private AttechPartCheckData _RightAttechPartCheckData;

    [Tooltip("아웃라인드래그앤 드롭")] [SerializeField] private StepOulineController StepOutline;

    private int _PartSize;
    private int _CurrentPartCount;
    private int _CurrentOutLineCount;
    private bool _IsComplete;
    private bool _IsOnOutLine;

    private bool _IsNext;
    private bool _IsPrev;
    private bool IsAnimation;

    public bool IsComplete { get { return _IsComplete; } }

    public int AssemblyPartSize { get { return _AssemblyPart.Count; } }
    public int CurrentPartCount { get { return _CurrentPartCount; } }
    public bool OutLineOn { get { return _IsOnOutLine; } }
    public int CurrentOutLineCount { get { return _CurrentOutLineCount; } }
    public AssemblyCheckMethod OnAssemblyCheckMethod { get { return _AssemblyCheckMethod; } }
    public AssemblyObjectAttech OnAssemblyObjectAttech { get { return _AssemblyObjectAttech; } }
    public AssemblyObjectDetech OnAssemblyObjectDetech { get { return _AssemblyObjectDetech; } }

    public AssemblyToolCheckMethod OnAssemblyToolCheckMethod { get { return _AssemblyToolCheckMethod; } }

    // Start is called before the first frame update
    void Start()
    {
        _IsOnOutLine = false;
        _PartSize = _AssemblyPart.Count;
        _CurrentPartCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsComplete.Equals(false))
        {
            if(_IsNext.Equals(false))
            {
                if (_LeftHandPart != null && _LeftAttechPartCheckData != null)
                    HandTypeUpdate(_LeftHandPart, _RightHandPart, _LeftAttechPartCheckData, _RightAttechPartCheckData);

                if (_RightHandPart != null && _RightAttechPartCheckData != null)
                    HandTypeUpdate(_RightHandPart, _LeftHandPart, _RightAttechPartCheckData, _LeftAttechPartCheckData);
            }
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init(AssemblyObject.AssemblyType assemblyType) // 생성된 부품들을 해당 위치로 이동 시켜 줌
    {
        _AssemblyCheckMethod += AssembleCheck;
        _AssemblyObjectAttech += AssembleObjectAttechCallBack;
        _AssemblyObjectDetech += AssembleObjectDetechCallBack;
        _AssemblyToolCheckMethod += AssemblyToolCheck;

        for (int i = 0; i < _AssemblyPart.Count; i++)
            _AssemblyPart[i].Init(this, assemblyType);

        for(int i = 0; i < _AssemblyKeypoint.Count; i++)
            _AssemblyKeypoint[i].Init(assemblyType);

        if (assemblyType.Equals(AssemblyObject.AssemblyType.Decomposition) && _AssemblyKeypoint.Count > 0)
        {
            for (int i = 0; i < _AssemblyPart.Count; i++)
            {
                if(i < _AssemblyKeypoint.Count)
                {
                    _AssemblyPart[i].DirectionSetup(_AssemblyKeypoint[i]);
                    _AssemblyPart[i].transform.position = _AssemblyKeypoint[i].transform.position;
                    _AssemblyPart[i].transform.rotation = _AssemblyKeypoint[i].transform.rotation;
                    _AssemblyPart[i].Position = _AssemblyKeypoint[i].transform.position;
                }
            }
            //for (int i = 0; i < _AssemblyPart.Count; i++)// for문 따로 돌려야 오류 미생성됨.
            //{
            //    _AssemblyPart[i].ActiveHightObject();
            //}
        }
    }

    /// <summary>
    /// 조립 시작알려줌
    /// </summary>
    public void AssemblyStart()
    {
        _IsComplete = false;
        _CurrentPartCount = 0;

        for (int i = 0; i < _AssemblyPart.Count; i++)
        {
            _AssemblyPart[i].AssembleStart();
            _AssemblyKeypoint[i].AssemblyStart();
        }
    }

    /// <summary>
    /// 현재단계 스킵해줌
    /// </summary>
    public void NextStep()
    {
        if(_IsNext.Equals(false))
        {
            for (int i = 0; i < _AssemblyPart.Count; i++)
            {
                _AssemblyPart[i].AssemblyNextSetup(_AssemblyKeypoint[i]);
                _AssemblyKeypoint[i].AssemblyNextSetup();
            }
            Debug.Log(_IsComplete);

            _CurrentPartCount = _AssemblyPart.Count;
            _IsComplete = true;
        }
    }

    /// <summary>
    /// 현재단계 진행사항 초기화
    /// </summary>
    public void PrevStep()
    {
        if(_IsPrev.Equals(false))
        {
            for (int i = 0; i < _AssemblyPart.Count; i++)
            {
                _AssemblyPart[i].AssemblyPrev();
                _AssemblyKeypoint[i].AssemblyPrev();
            }
            Debug.Log(_IsComplete);

            _CurrentPartCount = 0;
            _IsComplete = false;
        }
    }

    /// <summary>
    /// 조립 업데이트
    /// </summary>
    private void HandTypeUpdate(PartName _AttechPart, PartName _OtherPart, AttechPartCheckData checkData, AttechPartCheckData otherData)
    {

        if (_AttechPart._idx.Equals(IDX.Bolt))
        {
            var PartIndex = _AttechPart.states.CurrentStep;
            bool IsCheck = true;
            #region
            //1. 다른 오브젝트 있을경우
            //2. 현재 잡고있는 오브젝트의 고스트가 조립적정일때
            //3. 현재 잡고있는 오브젝트가 일치하는 오브젝트와 결합되지 않았을때
            //건너뛰기
            //if (_OtherPart != null && _AssemblyKeypoint[PartIndex].IsAssemblyCheck.Equals(true) && checkData._NomalAssembly.Equals(false))
            //    IsCheck = false;
            #endregion

            if ((!_AssemblyKeypoint[PartIndex].CheckHand.Equals(SteamVR_Input_Sources.Any) && !_AssemblyKeypoint[PartIndex].CheckHand.Equals(checkData._HandSource)))
                IsCheck = false;

            if ((_AssemblyPart[PartIndex].IsAssembleEnd.Equals(false) && _AssemblyKeypoint[PartIndex].IsAssembleEnd.Equals(false))
                && IsCheck.Equals(true))
            {

                float distance = Vector3.Distance(_AssemblyPart[PartIndex].Position, _AssemblyKeypoint[PartIndex].transform.position);
                //_AssemblyKeypoint 의 변수 포지션이 아닌 현재의 포지션을 가져온다 변수는 init으로 위치가 정해져 있기 때문이다
                if (distance < 0.04f)
                {
                    _AssemblyPart[PartIndex].IsAssembleCheck = true;
                    _AssemblyKeypoint[PartIndex].CheckHand = checkData._HandSource;
                    _AssemblyKeypoint[PartIndex].SetColor(true);
                    checkData._NomalAssembly = true;
                }
                else
                {
                    _AssemblyPart[PartIndex].IsAssembleCheck = false;
                    _AssemblyKeypoint[PartIndex].CheckHand = SteamVR_Input_Sources.Any;
                    _AssemblyKeypoint[PartIndex].SetColor(false);
                    checkData._NomalAssembly = false;
                }
            }

            if (checkData._NomalAssembly.Equals(false))
            {
                for (int j = 0; j < _AssemblyKeypoint.Count; j++)
                {
                    //1. 이미 대응되는 오브젝트가 아니므로 잡고있는 고스트는 생략한다
                    if (j == _AttechPart.states.CurrentStep)
                        continue;

                    if (!_AssemblyKeypoint[j].CheckHand.Equals(SteamVR_Input_Sources.Any) && !_AssemblyKeypoint[j].CheckHand.Equals(checkData._HandSource))
                        continue;

                    #region
                    // 1. 다른오브젝트가 있을경우
                    // 2. 현재 반복문에서 오브젝트가 조립적정일때
                    // 3. 다른 오브젝트가 정상조립 or 다른곳에 조립 하는 경우
                    // 4. 현재 아이템이 상대방 꺼가 아닐경우
                    // 건너뛰기
                    //if(_OtherPart != null)
                    //{
                    //    if (_AssemblyKeypoint[j].IsAssemblyCheck.Equals(true) && (otherData._NomalAssembly || otherData._NotMatcingAssembly)
                    //        && !j.Equals(checkData._NotMactingKeypoint))
                    //    {
                    //        continue;
                    //    }
                    //}
                    #endregion

                    if (_AssemblyKeypoint[j].IsAssembleEnd.Equals(false) &&
                        _AttechPart.states.ObjectName.Equals(_AssemblyKeypoint[j].PartName.states.ObjectName))
                    {
                        float distance_2 = Vector3.Distance(_AssemblyPart[_AttechPart.states.CurrentStep].Position, _AssemblyKeypoint[j].Position);

                        if (distance_2 < 0.04f)
                        {
                            _AssemblyPart[_AttechPart.states.CurrentStep].IsAssembleCheck = true;
                            _AssemblyKeypoint[j].CheckHand = checkData._HandSource;

                            _AssemblyKeypoint[j].SetColor(true);
                            checkData._NotMatcingAssembly = true;
                            checkData._NotMactingKeypoint = j;
                            break;
                        }
                        else
                        {
                            _AssemblyPart[_AttechPart.states.CurrentStep].IsAssembleCheck = false;
                            _AssemblyKeypoint[j].CheckHand = SteamVR_Input_Sources.Any;
                            _AssemblyKeypoint[j].SetColor(false);

                            checkData._NotMatcingAssembly = false;
                            checkData._NotMactingKeypoint = -1;
                        }
                    }
                }
            }
        }

        if (_AttechPart._idx.Equals(IDX.Object))
        {
            var PartIndex = _AttechPart.states.CurrentStep;
            bool IsCheck = true;
            #region
            //1. 다른 오브젝트 있을경우
            //2. 현재 잡고있는 오브젝트의 고스트가 조립적정일때
            //3. 현재 잡고있는 오브젝트가 일치하는 오브젝트와 결합되지 않았을때
            //건너뛰기
            //if (_OtherPart != null && _AssemblyKeypoint[PartIndex].IsAssemblyCheck.Equals(true) && checkData._NomalAssembly.Equals(false))
            //    IsCheck = false;
            #endregion

            if ((!_AssemblyKeypoint[PartIndex].CheckHand.Equals(SteamVR_Input_Sources.Any) && !_AssemblyKeypoint[PartIndex].CheckHand.Equals(checkData._HandSource)))
                IsCheck = false;

            if ((_AssemblyPart[PartIndex].IsAssembleEnd.Equals(false) && _AssemblyKeypoint[PartIndex].IsAssembleEnd.Equals(false))
                && IsCheck.Equals(true))
            {

                float distance = Vector3.Distance(_AssemblyPart[PartIndex].Position, _AssemblyKeypoint[PartIndex].transform.position);
                float Rot = Quaternion.Angle(_AssemblyPart[PartIndex].transform.rotation, _AssemblyKeypoint[PartIndex].transform.rotation);
                //_AssemblyKeypoint 의 변수 포지션이 아닌 현재의 포지션을 가져온다 변수는 init으로 위치가 정해져 있기 때문이다
                if (distance < 0.04f && Rot < 20.0f)
                {
                    _AssemblyPart[PartIndex].IsAssembleCheck = true;
                    _AssemblyKeypoint[PartIndex].CheckHand = checkData._HandSource;
                    _AssemblyKeypoint[PartIndex].SetColor(true);
                    checkData._NomalAssembly = true;
                }
                else
                {
                    _AssemblyPart[PartIndex].IsAssembleCheck = false;
                    _AssemblyKeypoint[PartIndex].CheckHand = SteamVR_Input_Sources.Any;
                    _AssemblyKeypoint[PartIndex].SetColor(false);
                    checkData._NomalAssembly = false;
                }
            }

            if (checkData._NomalAssembly.Equals(false))
            {
                for (int j = 0; j < _AssemblyKeypoint.Count; j++)
                {
                    //1. 이미 대응되는 오브젝트가 아니므로 잡고있는 고스트는 생략한다
                    if (j == _AttechPart.states.CurrentStep)
                        continue;

                    if (!_AssemblyKeypoint[j].CheckHand.Equals(SteamVR_Input_Sources.Any) && !_AssemblyKeypoint[j].CheckHand.Equals(checkData._HandSource))
                        continue;

                    #region
                    // 1. 다른오브젝트가 있을경우
                    // 2. 현재 반복문에서 오브젝트가 조립적정일때
                    // 3. 다른 오브젝트가 정상조립 or 다른곳에 조립 하는 경우
                    // 4. 현재 아이템이 상대방 꺼가 아닐경우
                    // 건너뛰기
                    //if(_OtherPart != null)
                    //{
                    //    if (_AssemblyKeypoint[j].IsAssemblyCheck.Equals(true) && (otherData._NomalAssembly || otherData._NotMatcingAssembly)
                    //        && !j.Equals(checkData._NotMactingKeypoint))
                    //    {
                    //        continue;
                    //    }
                    //}
                    #endregion
                    if (_AssemblyKeypoint[j].IsAssembleEnd.Equals(false) &&
                        _AttechPart.states.ObjectName.Equals(_AssemblyKeypoint[j].PartName.states.ObjectName))
                    {
                        float distance_2 = Vector3.Distance(_AssemblyPart[_AttechPart.states.CurrentStep].Position, _AssemblyKeypoint[j].Position);
                        float Rot = Quaternion.Angle(_AssemblyPart[PartIndex].transform.rotation, _AssemblyKeypoint[PartIndex].transform.rotation);

                        if (distance_2 < 0.04f && Rot < 20.0f)
                        {
                            _AssemblyPart[_AttechPart.states.CurrentStep].IsAssembleCheck = true;
                            _AssemblyKeypoint[j].CheckHand = checkData._HandSource;

                            _AssemblyKeypoint[j].SetColor(true);
                            checkData._NotMatcingAssembly = true;
                            checkData._NotMactingKeypoint = j;
                            Debug.Log("Drop");
                            break;
                        }
                        else
                        {
                            _AssemblyPart[_AttechPart.states.CurrentStep].IsAssembleCheck = false;
                            _AssemblyKeypoint[j].CheckHand = SteamVR_Input_Sources.Any;
                            _AssemblyKeypoint[j].SetColor(false);

                            checkData._NotMatcingAssembly = false;
                            checkData._NotMactingKeypoint = -1;
                        }
                    }
                }
            }
        }

      
    }

    private void AssemblyToolCheck()
    {
        _CurrentOutLineCount++;
        _CurrentPartCount++;

        if (_CurrentPartCount >= _PartSize)
        {
            _IsComplete = true;
        }
        if (StepOutline != null && Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal)) // 다음 볼트 아웃라인 표시
        {
            StepOutline.OutLineStep(_CurrentOutLineCount);
        }

    }

    #region CallBack

    /// <summary>
    /// 현재 단계 완료체크
    /// </summary>
    /// <param name="AssembleIndex"></param>
    private void AssembleCheck(int AssembleIndex, SteamVR_Input_Sources type)
    {
        var CheckData = type.Equals(SteamVR_Input_Sources.LeftHand) ? _LeftAttechPartCheckData 
            : type.Equals(SteamVR_Input_Sources.RightHand) ? _RightAttechPartCheckData : null;

        int PartIndex = AssembleIndex;
        int KeypointIndex = AssembleIndex;

        Directory.Instance.soundManager.PlaySound(Directory.Instance.soundManager._Drop);
        //Director.Instance.SoundManager.PlayConnectSound(SoundManager.ConnectSoundIndex.Connect_S001);

        if(CheckData != null && CheckData._NotMatcingAssembly.Equals(true))
        {
            PartIndex = AssembleIndex;
            KeypointIndex = CheckData._NotMactingKeypoint;
        }

        _AssemblyPart[PartIndex].AssembleEnd();
        _AssemblyPart[PartIndex].DirectionSetup(_AssemblyKeypoint[KeypointIndex]);
        _AssemblyPart[PartIndex].ToolTypeSetup(_AssemblyKeypoint[KeypointIndex].ToolType);
        _AssemblyKeypoint[KeypointIndex].AssembleEnd();
        // 부모객체로 옮겨주는 작업의 예외처리가 필요할듯 하다
        _AssemblyPart[PartIndex].transform.SetParent(_AssemblyKeypoint[KeypointIndex].transform.parent);

        _AssemblyPart[PartIndex].transform.position = _AssemblyKeypoint[KeypointIndex].transform.position;
        _AssemblyPart[PartIndex].transform.rotation = _AssemblyKeypoint[KeypointIndex].transform.rotation;

        _AssemblyPart[PartIndex].Position = _AssemblyPart[PartIndex].transform.position;
        _AssemblyKeypoint[KeypointIndex].gameObject.SetActive(false);

        if (_AssemblyPart[PartIndex].ObjectPartType.Equals(PartType.Tool))
        {
            _CurrentPartCount++;
            if (_CurrentPartCount >= _PartSize)
            {
                _IsOnOutLine = true;
                _CurrentPartCount = 0;
                StepOutline.SetNotOrderOutLine();
                StepOutline.OrderFirstOutLineStep(); // 볼트 아웃라인 시작
            }
        }

        if (!_AssemblyPart[PartIndex].ObjectPartType.Equals(PartType.Tool))
        {
            _CurrentPartCount++;
            if (_CurrentPartCount >= _PartSize)
            {
                _IsComplete = true;
            }
        }

        if (!_AssemblyPart[PartIndex].ObjectPartType.Equals(PartType.Animation))
        {
            SteamVR_Input_Sources hand = type;

            int AttechPartIndex = AssembleIndex;

            bool IsNomalAssembly = CheckData._NomalAssembly;
            bool IsNotMatcingAssembly = CheckData._NotMatcingAssembly;
            int NotMactcingkeypoint = CheckData._NotMactingKeypoint;

            object[] datas = { hand, AttechPartIndex, IsNomalAssembly, IsNotMatcingAssembly, NotMactcingkeypoint };
        }
    }

    private void AssembleObjectAttechCallBack(PartName part, SteamVR_Input_Sources handType)
    {
        if(handType.Equals(SteamVR_Input_Sources.LeftHand))
        {
            _LeftHandPart = part;
            _LeftAttechPartCheckData = new AttechPartCheckData();
            _LeftAttechPartCheckData._HandSource = handType;
        }
        else if(handType.Equals(SteamVR_Input_Sources.RightHand))
        {
            _RightHandPart = part;
            _RightAttechPartCheckData = new AttechPartCheckData();
            _RightAttechPartCheckData._HandSource = handType;
        }
    }

    private void AssembleObjectDetechCallBack(PartName part, SteamVR_Input_Sources handType)
    {
        if (handType.Equals(SteamVR_Input_Sources.LeftHand))
        {
            _LeftHandPart = null;
        }
        else if (handType.Equals(SteamVR_Input_Sources.RightHand))
        {
            _RightHandPart = null;
        }
    }

    #endregion
}

#region 왜했지
//Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
//{
//    float gravity = Physics.gravity.magnitude;
//    float angle = initialAngle * Mathf.Deg2Rad;

//    Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
//    Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

//    float distance = Vector3.Distance(planarTarget, planarPosition);
//    float yOffset = currentPos.y - targetPos.y;

//    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

//    Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

//    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
//    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

//    return finalVelocity;
//}
#endregion