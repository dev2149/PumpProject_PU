using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonitorView : MonoBehaviour
{
    [SerializeField] Image _ViewMonitor;
    // Start is called before the first frame update
    void Start()
    {
        _ViewMonitor = GetComponent<Image>();
    }
    public void SetMonitorImageChange(Sprite spr)
    {
        if(spr != null)
        {
            _ViewMonitor.sprite = spr;
        }
        else
        {
            Debug.Log("null");
        }
    }
}
