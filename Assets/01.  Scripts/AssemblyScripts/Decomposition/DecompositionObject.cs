using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecompositionObject : MonoBehaviour
{
    public enum RemoveType
    {
        Animation,
        Fade
    }

    [SerializeField] private List<DecompositionStep> _DecompositionSteps;

    private bool _IsComplete;

    private int _CompleteStepCount;

    public bool IsNext
    {
        get
        {
            if (_CompleteStepCount < _DecompositionSteps.Count)
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
            if (_CompleteStepCount > 0)
            {
                return true;
            }

            return false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _DecompositionSteps.Count; i++)
        {
            _DecompositionSteps[i].Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnClickNext();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnClickPrev();
        }

        int temp = 0;

        for (int i = 0; i < _DecompositionSteps.Count; i++)
        {
            if (_DecompositionSteps[i].IsComplete.Equals(true))
            {
                temp++;
            }
        }

        _CompleteStepCount = temp;
    }

    #region

    public void OnClickNext()
    {
        if (IsNext.Equals(true))
        {
            for(int i = 0; i < _DecompositionSteps.Count; i++)
            {
                if (_DecompositionSteps[i].IsComplete.Equals(false))
                {
                    _DecompositionSteps[i].NextStep();

                    break;
                }
            }
        }
    }

    public void OnClickPrev()
    {
        if (IsPrev.Equals(true))
        {
            for (int i = _DecompositionSteps.Count-1; i >= 0; i--)
            {
                if (_DecompositionSteps[i].IsComplete.Equals(true))
                {
                    _DecompositionSteps[i].PrevStep();

                    break;
                }
            }                                                    
        }
    }

    #endregion
}