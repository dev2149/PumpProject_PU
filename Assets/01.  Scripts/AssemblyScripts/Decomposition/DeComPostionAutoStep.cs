using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeComPostionAutoStep : MonoBehaviour
{
    [Tooltip("앞의 오브젝트들이 분해가 완료 되었는지\n분해 순서 스크립트 드래그 앤 드롭 으로 추가")]
    [SerializeField] DecompositionPartObject _CheckDecomPosition;
    [SerializeField] GameObject[] _DecomPositonObj;
    private float _CurrentTime;
    private float _Deltatime;
    public bool _CheckDelete;
    private void Start()
    {
        _Deltatime = 3.7f;
        _CheckDelete = false;
    }
    private void Update()
    {
        if (_CheckDecomPosition.IsDetechObject && !_CheckDelete)
        {
            _CurrentTime += Time.deltaTime;
            StartCoroutine(DelayDecomPositionMove());
            if (_Deltatime <= _CurrentTime)
            {
                StopAllCoroutines();
                DeComPositionNextStep();
                _CheckDelete = true;
            }
        }
    }
    IEnumerator DelayDecomPositionMove()
    {
        for (int i = 0; i < _DecomPositonObj.Length; i++)
        {
            float _vectorX = _DecomPositonObj[i].GetComponent<AssemblyPart>().Direction.x;
            float _vectorY = _DecomPositonObj[i].GetComponent<AssemblyPart>().Direction.y;
            _DecomPositonObj[i].transform.position = new Vector3(_DecomPositonObj[i].transform.position.x + Time.deltaTime * 0.2f * _vectorX, _DecomPositonObj[i].transform.position.y + Time.deltaTime * 0.2f * _vectorY, _DecomPositonObj[i].transform.position.z);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void DeComPositionNextStep()
    {
        for (int i = 0; i < _DecomPositonObj.Length; i++)
        {
            _DecomPositonObj[i].GetComponent<AssemblyPart>().DecompositionObject();
        }
    }
}