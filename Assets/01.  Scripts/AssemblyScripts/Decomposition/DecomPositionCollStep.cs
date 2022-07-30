using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomPositionCollStep : MonoBehaviour
{
    [Tooltip("앞의 오브젝트들이 분해가 완료 되었는지\n분해 순서 스크립트 드래그 앤 드롭 으로 추가")]
    [SerializeField] DecompositionPartObject _CheckDecomPosition;
    public BoxCollider[] _PartCollider;
    public GameObject[] _HightLightObject;

    bool _CheckNextStep;
    void Awake()
    {
        _PartCollider = this.gameObject.GetComponentsInChildren<BoxCollider>();
        _CheckNextStep = false;
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            for (int i = 0; i < _HightLightObject.Length; i++)
            {
                _HightLightObject[i].gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (_CheckDecomPosition != null)
        {
            if (!_CheckNextStep && _CheckDecomPosition.IsDetechObject && Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
            {
                if (_HightLightObject != null)
                {
                    for (int i = 0; i < _HightLightObject.Length; i++)
                    {
                        _HightLightObject[i].gameObject.SetActive(true);
                    }
                }
                _CheckNextStep = true;

            }

            if(!_CheckNextStep && _CheckDecomPosition.IsDetechObject && Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
            {
                if (_HightLightObject != null)
                {
                    for (int i = 0; i < _HightLightObject.Length; i++)
                    {
                        _HightLightObject[i].gameObject.SetActive(false);
                    }
                }
                _CheckNextStep = true;
            }
        }
    }
    public void ActiveCollider()
    {
        for (int i = 0; i < _PartCollider.Length; i++)
        {
            _PartCollider[i].gameObject.SetActive(true);
        }
    }
    public void DeletCollider()
    {
        for (int i = 0; i < _PartCollider.Length; i++)
        {
            _PartCollider[i].gameObject.SetActive(false);
        }
        if (_HightLightObject != null)
        {
            for (int i = 0; i < _HightLightObject.Length; i++)
            {
                _HightLightObject[i].gameObject.SetActive(false);
            }
        }
    }
}
