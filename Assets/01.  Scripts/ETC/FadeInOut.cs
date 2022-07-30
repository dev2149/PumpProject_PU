using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image m_TitlePanel;
    float create_timer = 2.0f;
    float create_waiting_Time = 0.0f;

    void Start()
    {
        ChildLaod();
    }
    void ChildLaod()
    {
        m_TitlePanel = GameObject.Find("PlayerForwardCanvas").gameObject.transform.Find("Panel").GetComponent<Image>();
    }

    void Update()
    {
        create_waiting_Time += Time.deltaTime;
        if (create_timer <= create_waiting_Time)
        {
            StartCoroutine(TitleImagesFID());
            create_waiting_Time = 0.0f;
        }
    }
    IEnumerator TitleImagesFID()
    {
        while (m_TitlePanel.color.a > 0.0f)
        {
            m_TitlePanel.color = new Color(m_TitlePanel.color.r, m_TitlePanel.color.g, m_TitlePanel.color.b,
                m_TitlePanel.color.a - (Time.deltaTime / create_timer));
            yield return null;
        }
        m_TitlePanel.gameObject.SetActive(false);
        Directory.Instance.sceneController.LoadScene(SceneIdx.Title);
    }
}
