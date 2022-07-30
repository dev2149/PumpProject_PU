using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
public class VRInputModule : BaseInputModule
{
    public Camera m_cam;
    public SteamVR_Input_Sources m_TartgetSouce;
    public SteamVR_Action_Boolean m_ClickAction;

    [SerializeField] private GameObject m_CurrentObj = null;
    [SerializeField] private PointerEventData m_Data = null;

    protected override void Awake()
    {
        base.Awake();
        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // Reset Data
        m_Data.Reset();
        m_Data.position = new Vector2(m_cam.pixelWidth / 2, m_cam.pixelHeight / 2);

        // RayCast
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObj = m_Data.pointerCurrentRaycast.gameObject;

        //crear
        m_RaycastResultCache.Clear();

        //Hover
        HandlePointerExitAndEnter(m_Data, m_CurrentObj);

        //press
        if (m_ClickAction.GetStateDown(m_TartgetSouce))
            ProcessPress(m_Data);

        //Release
        if (m_ClickAction.GetStateUp(m_TartgetSouce))
            ProcessRelease(m_Data);
    }

    public PointerEventData GetData()
    {
        return m_Data;
    }

    private void ProcessPress(PointerEventData data)
    {
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObj, data, ExecuteEvents.pointerDownHandler);

        if (newPointerPress == null)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObj);


        }

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = m_CurrentObj;
    }
    private void ProcessRelease(PointerEventData data)
    {
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObj);
        // m_CurrentObj의 종류에 의해 상호작용 스크립트 작성 필요
        if (data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);

        }
        eventSystem.SetSelectedGameObject(null);
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
