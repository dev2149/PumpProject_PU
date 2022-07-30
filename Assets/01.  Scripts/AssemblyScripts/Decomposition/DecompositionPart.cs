using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DecompositionPart : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {

    }

    #region 

    public void NextSetup()
    {
        this.gameObject.SetActive(false);
    }

    public void PrevSetup()
    {
        this.gameObject.SetActive(true);
    }

    #endregion
}
