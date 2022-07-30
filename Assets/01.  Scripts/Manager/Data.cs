using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Rootobject
{
    public Step[] step;
}
[System.Serializable]
public class Step
{
    public int id;
    public string name;
    public string explanation0;
    public Step() { }
    public Step(Step _info)
    {
        id = _info.id;
        name = _info.name;
        explanation0 = _info.explanation0;
    }
}
