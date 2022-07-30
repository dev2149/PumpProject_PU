using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTargetController : MonoBehaviour
{
    LineRenderer lr;
    [SerializeField] GameObject[] _SphereGroup;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;

    }

    void Update()
    {
        lr.SetPosition(0, _SphereGroup[0].GetComponent<Transform>().position);
        lr.SetPosition(1, _SphereGroup[1].GetComponent<Transform>().position);
    }

}
