using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float m_DefeaultLength = 5.0f;
    public GameObject m_Dot;
    public VRInputModule m_InputModule;

    private LineRenderer m_LineRenderer = null;

    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdataLine();// 트리거를 눌렸을때 라인렌더러가 나오도록 수정 필요.
                     // 라인렌더러가 오브젝트를 감지 햇을때와 하지 않았을때의 차이 색상으로 변경 필요
    }
    void UpdataLine()
    {
        PointerEventData data = m_InputModule.GetData();

        float targetLenth = data.pointerCurrentRaycast.distance == 0 ? m_DefeaultLength : data.pointerCurrentRaycast.distance;
        RaycastHit hit = CreateRaycast(targetLenth);
        Vector3 endPosition = transform.position + (transform.forward * targetLenth);

        if (hit.collider != null)
            endPosition = hit.point;

        m_Dot.transform.position = endPosition;

        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPosition);

    }
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefeaultLength);

        return hit;
    }
}
