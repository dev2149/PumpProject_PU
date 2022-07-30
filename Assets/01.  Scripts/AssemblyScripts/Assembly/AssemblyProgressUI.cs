using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyProgressUI : MonoBehaviour
{
    [SerializeField] private Image _ProgressImage;
    [SerializeField] private GameObject _Back;

    public float Progress
    {
        set
        {
            _ProgressImage.fillAmount = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUse(bool isShow)
    {
        _Back.SetActive(isShow);
    }
}
