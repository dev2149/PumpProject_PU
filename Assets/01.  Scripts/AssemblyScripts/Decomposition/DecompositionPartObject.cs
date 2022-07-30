using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecompositionPartObject : MonoBehaviour
{
    [SerializeField] private DetechType _DetechType;
    [SerializeField] private List<AssemblyPart> _AssemblyParts;

    private Animator _Animator;
    private bool _IsDetechObject;

    public DetechType DetechTypeData { get { return _DetechType; } }
    public bool IsDetechObject { get { return _IsDetechObject; } }

    public enum DetechType
    {
        Animation,
        Fade,
        Auto
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(_AssemblyParts != null && _IsDetechObject.Equals(false))
        {
            int index = 0;

            for(int i = 0; i < _AssemblyParts.Count; i++)
            {
                if (_AssemblyParts[i].IsToolEnd.Equals(true))
                    index++;
            }

            if (index.Equals(_AssemblyParts.Count))
                _IsDetechObject = true;
        }
    }

    public void SetPartAnimation()
    {
        if(_Animator != null)
            _Animator.SetTrigger("PartAnimation");
    }
}