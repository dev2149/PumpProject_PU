using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToCamController : MonoBehaviour
{
    [SerializeField] private Camera m_mainCam;
    // Start is called before the first frame update
    void Start()
    {
        m_mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + m_mainCam.transform.rotation * Vector3.back);
    }
}
