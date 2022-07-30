using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeLayerChange : MonoBehaviour
{
    void Start()
    {
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            this.gameObject.tag = "AssemblyPartToolCheckCollider";
        }
    }
}
