using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DecomPositionUILogic : MonoBehaviour
{
    void Start()
    {
        
    }
    
    public void CloasePanel()
    {
        Debug.Log(this.gameObject.transform.name);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
