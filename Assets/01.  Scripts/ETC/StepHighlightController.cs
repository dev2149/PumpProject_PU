using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHighlightController : MonoBehaviour
{
    [SerializeField] Material m_MyMaterial;
    float m_ThruVal;
    private void Start()
    {
        m_ThruVal = m_MyMaterial.GetFloat("_SeeThru");
        StartCoroutine(StartHighLight());
    }
    IEnumerator StartHighLight()
    {
        while(m_ThruVal > 0.0f)
        {
            m_MyMaterial.SetFloat("_SeeThru", m_ThruVal -= (Time.deltaTime * 1.0f));
            yield return null;
        }
        StartCoroutine(EndHighLight());
    }
    IEnumerator EndHighLight()
    {
        while (m_ThruVal < 1.0f)
        {
            m_MyMaterial.SetFloat("_SeeThru", m_ThruVal += (Time.deltaTime * 1.0f));
            yield return null;
        }
        StartCoroutine(StartHighLight());
    }

}
