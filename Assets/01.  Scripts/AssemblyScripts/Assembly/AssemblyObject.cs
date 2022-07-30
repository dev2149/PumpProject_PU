using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyObject : MonoBehaviour
{
    public enum AssemblyType
    {
        Assembly,
        Decomposition
    }

    private delegate void AssemblyConnect();

    /// <summary>
    /// Temp 분해 다되는거 체크
    /// </summary>
    [SerializeField] private AssemblyPart[] _DecompositionObjects;
    [SerializeField] private Animator _Animator;

    [SerializeField] private AssemblyType _AssemblyType;
    [SerializeField] private List<AssemblyStep> _AssemblyStep = new List<AssemblyStep>();
    [SerializeField] private List<AssemblyStep> _HighLightStep = new List<AssemblyStep>();
    [SerializeField] private List<DecomPositionCollStep> _DecomPositionStep = new List<DecomPositionCollStep>();
    //[SerializeField] private EndPopupUI _EndPopupUI;
    [SerializeField] private float _Processivity;
    // Assembly
    [SerializeField] private AssemblyStep _CurrentStep;
    private int _CurrentStepCount;
    private int _StepSize;
    private int _AssemblyObjectSize;
    public float Processivity { get { return _Processivity; } }
    public bool _TutorialCheck = false;

    public bool IsNext
    {
        get
        {
            if (_CurrentStepCount < _AssemblyStep.Count)
            {
                return true;
            }

            return false;
        }
    }
    public bool IsPrev
    {
        get
        {
            if (_CurrentStepCount > 0)
            {
                return true;
            }

            return false;
        }
    }
    void Start()
    {
        for (int i = 0; i < _AssemblyStep.Count; i++)
        {
            _AssemblyObjectSize += _AssemblyStep[i].AssemblyPartSize;

            _AssemblyStep[i].Init(_AssemblyType);
            _AssemblyStep[i].gameObject.SetActive(_AssemblyType.Equals(AssemblyType.Assembly) ? false : true);

            Debug.Log(_AssemblyStep[i]);
        }
        if (_AssemblyType.Equals(AssemblyType.Assembly))
        {
            _StepSize = _AssemblyStep.Count;
            _CurrentStepCount = 0;
            _CurrentStep = _AssemblyStep[_CurrentStepCount];
            _CurrentStep.AssemblyStart();
            _CurrentStep.gameObject.SetActive(true);
        }
        for (int i = 1; i < _DecomPositionStep.Count; i++)// 1부터 시작
        {
            _DecomPositionStep[i].DeletCollider();
        }
    }
    void Update()
    {
        if (_AssemblyType.Equals(AssemblyType.Assembly))
        {
            if (_CurrentStep != null)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnClickNext();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnClickPrev();
                }

                AssemblyPartConnectCheck();
                CompleteCheck();
            }
        }
        else if (_AssemblyType.Equals(AssemblyType.Decomposition))
        {
            int index = 0;

            for (int i = 0; i < _DecompositionObjects.Length; i++)
            {
                if (_DecompositionObjects[i].IsDetechCheck.Equals(true))
                {
                    index++;
                }
            }

            _Processivity = ((float)index / (float)_DecompositionObjects.Length) * 100;
            if (index.Equals(_DecompositionObjects.Length))
            {
                //_Animator.SetTrigger("PartAnimation");
            }
            DecomPositionStepActiveCollider(index);// 다음파트 넘어갈때

        }
    }

    /// <summary>
    /// 전체카운드에 현재카운트를 임시변수에 넣어서 계산
    /// </summary>
    private void AssemblyPartConnectCheck()
    {
        int temp = 0;

        for (int i = 0; i < _AssemblyStep.Count; i++)
        {
            if (_AssemblyStep[i].IsComplete)
            {
                temp += _AssemblyStep[i].CurrentPartCount;
            }
        }
        _Processivity = ((float)temp / (float)_AssemblyObjectSize) * 100;
    }

    /// <summary>
    /// 완료 체크
    /// </summary>
    private void CompleteCheck()
    {

        if (_CurrentStep.IsComplete.Equals(true))
        {
            _CurrentStepCount++; // 다음 스텝 카운트
            _TutorialCheck = true;
            if (_CurrentStepCount < _StepSize)
            {
                //_CurrentStep.gameObject.SetActive(false); //이거 왜 넣었지?

                _CurrentStep = _AssemblyStep[_CurrentStepCount];
                _CurrentStep.AssemblyStart();
                _CurrentStep.gameObject.SetActive(true);
            }
            else
            {
                _CurrentStep = null;
                
                //Director.Instance.SoundManager.PlayEffectSound(SoundManager.EffectSoundIndex.Complete);

                //if (!Director.Instance.GameSceneManager._CurrentGameSceneIndex.Equals(GameSceneManager.GameSceneIndex.Tutorial))
                //    _EndPopupUI.Show();
            }
        }
    }
    public void OnClickNext()
    {
        if (IsNext)
        {
            _CurrentStep.NextStep();

        }
    }
    public void OnClickPrev()
    {
        if (IsPrev)
        {
            _CurrentStep.PrevStep();
            _CurrentStep.gameObject.SetActive(false);

            _CurrentStepCount--;
            _CurrentStep = _AssemblyStep[_CurrentStepCount];
            _CurrentStep.AssemblyStart();
            _CurrentStep.gameObject.SetActive(true);
        }
    }
    public void DecomPositionStepActiveCollider(int _count)
    {
        if (!_count.Equals(_DecompositionObjects.Length) && _count > 0)// 무엇인가 잘못됨
        {
            _DecomPositionStep[_count].ActiveCollider();
            _DecomPositionStep[_count - 1].DeletCollider();
        }
    }
    
    public void DecompositionComplete()
    {
        //Director.Instance.SoundManager.PlayEffectSound(SoundManager.EffectSoundIndex.Complete);

        //if (!Director.Instance.GameSceneManager._CurrentGameSceneIndex.Equals(GameSceneManager.GameSceneIndex.Tutorial))
        //    _EndPopupUI.Show();
    }
}