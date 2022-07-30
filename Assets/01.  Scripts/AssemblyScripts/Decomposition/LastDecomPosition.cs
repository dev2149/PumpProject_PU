using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDecomPosition : MonoBehaviour
{
    [Tooltip("분해 마지막 오브젝트 \n 드래그 앤 드롭 으로 추가")]
    [SerializeField] private DeComPostionAutoStep _LastDecomPosition;
    [SerializeField] GameObject[] _DecomPositonObj;
    [SerializeField] GameObject[] _DecomPositonObj_X;
    [SerializeField] GameObject[] _DecomPositonObj_Y;

    private float _CurrentTime;
    private float _Deltatime;
    public bool _CheckDelete;
    private void Start()
    {
        _Deltatime = 2.5f;
        _CheckDelete = false;
    }
    void Update()
    {
        if (_LastDecomPosition._CheckDelete && !_CheckDelete)
        {
            _CurrentTime += Time.deltaTime;
            StartCoroutine(DelayObjectPositionMoveY());
            StartCoroutine(DelayObjectPositionMoveX());
            if (_Deltatime <= _CurrentTime)
            {
                StopAllCoroutines();
                DeComPositionNextStep();
                _CheckDelete = true;
            }
        }
    }
    IEnumerator DelayObjectPositionMoveX()// 좌우 오브젝트 이동
    {
        for (int i = 0; i < _DecomPositonObj_X.Length; i++)
        {
            float _vector = _DecomPositonObj_X[i].GetComponent<AssemblyPart>().Direction.x;
            _DecomPositonObj_X[i].transform.position = new Vector3(_DecomPositonObj_X[i].transform.position.x + Time.deltaTime * 0.2f * _vector, _DecomPositonObj_X[i].transform.position.y, _DecomPositonObj_X[i].transform.position.z);
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator DelayObjectPositionMoveY()// 상하 오브젝트 이동
    {
        for (int i = 0; i < _DecomPositonObj_Y.Length; i++)
        {
            float _vector = _DecomPositonObj_Y[i].GetComponent<AssemblyPart>().Direction.y;
            _DecomPositonObj_Y[i].transform.position = new Vector3(_DecomPositonObj_Y[i].transform.position.x, _DecomPositonObj_Y[i].transform.position.y + Time.deltaTime * 0.07f * _vector, _DecomPositonObj_Y[i].transform.position.z);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void DeComPositionNextStep()// 모든 오브젝트 위치 이동
    {
        for (int i = 0; i < _DecomPositonObj.Length; i++)
        {
            _DecomPositonObj[i].GetComponent<AssemblyPart>().DecompositionObject();
        }
    }
}
