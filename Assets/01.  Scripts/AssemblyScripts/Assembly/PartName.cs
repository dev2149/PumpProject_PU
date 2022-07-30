using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum IDX
{
    Object,
    Bolt
}
public class PartName : MonoBehaviour
{
    [System.Serializable]
    public struct States
    {
        public string PartName;
        public string ObjectName;
        public int CurrentStep;
        public string AllStep;
        public string Seqence;
        public float BoltHeadDiameter;     
        public ElecImpact.Type ToolType;
        public float BoltDiameter;
        public float BoltDepth;
    }
    public States states;
    public IDX _idx;
    // Start is called before the first frame update
    public void Init()
    {
        string ObjectName = this.gameObject.name;

        string[] Temp = ObjectName.Split('_');
        if (Temp.Length.Equals(4))
        {
            states.PartName = Temp[0];
            states.ObjectName = Temp[1];
            states.CurrentStep = int.Parse(Temp[2])-1;
            states.Seqence = Temp[3];
        }
        else if (Temp.Length.Equals(8))
        {
            states.PartName = Temp[0];
            states.ObjectName = Temp[1];
            states.CurrentStep = int.Parse(Temp[2]) - 1;
            states.Seqence = Temp[3];

            states.BoltHeadDiameter = float.Parse(Temp[4].Replace("mm", " "));
            states.ToolType = (Tool.Type)System.Enum.Parse(typeof(Tool.Type), Temp[5]);
            states.BoltDiameter = float.Parse(Temp[6].Replace("mm", " "));
            states.BoltDepth = float.Parse(Temp[7].Replace("mm"," "));
        }
    }
}