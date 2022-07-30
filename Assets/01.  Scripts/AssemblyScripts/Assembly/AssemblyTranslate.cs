using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyTranslate : MonoBehaviour
{
    [SerializeField] private Transform _DummyObject;
    [SerializeField] private Vector3 _Direction;
    [SerializeField] private float _Value;

    private Quaternion _Reset;
    private Quaternion _Target;

    private bool _IsTranslate;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_IsTranslate)
        {
            if(Vector3.Distance(_DummyObject.rotation.eulerAngles, _Target.eulerAngles) <= 0.01f)
            {
                _IsTranslate = false;
            }

            _DummyObject.rotation = Quaternion.Lerp(_DummyObject.rotation, _Target, Time.deltaTime * 7.0f);
        }
    }

    public void Init()
    {
        _Reset = _DummyObject.rotation;
    }

    public void ResetData()
    {
        _IsTranslate = false;
        _DummyObject.rotation = _Reset;
    }

    public void Set()
    {
        if(!_IsTranslate)
        {
            _IsTranslate = true;

            _Target = Quaternion.Euler((_DummyObject.rotation.eulerAngles) + (_Direction * _Value));
        }
    }
}
