using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class TutorialLogic : MonoBehaviour
{
    public enum StageIndex 
    { 
        Teloprt,
        TriggerObject,
        Tool,
        Assembly,
        Compositon,
        Menu,
        Clear
    }
    // Teleport
    [SerializeField] private TeleportPoint _PointA;
    [SerializeField] private TeleportPoint _PointB;
    [SerializeField] private TeleportPoint _PointC;
    [SerializeField] private Teleport _Teleport;
    // Button
    [SerializeField] private SteamVR_Action_Boolean teleport;
    [SerializeField] private SteamVR_Action_Boolean _Trigger;
    [SerializeField] private SteamVR_Action_Boolean _Grab;
    [SerializeField] private SteamVR_Action_Boolean _Menu;
    // 이미지 변경
    [SerializeField] Sprite[] _TutorialImages;
    [SerializeField] MonitorView _monitorView;
    // 트리거 활용 오브젝트 잡기
    [SerializeField] GameObject[] _ActiveObject;
    [SerializeField] private AssemblyObject _AssemblyObject;
    [SerializeField] private DecompositionPartObject _Composition;
    [SerializeField] GameObject PumpBody;
    float narration_lenght_temp = 0.0f;
    [SerializeField] GameObject _PlayerObj;

    private void Start()
    {
        ChildLoad();
        StartCoroutine(TeleportLogic());
    }

    void ChildLoad()
    {
        _monitorView = GameObject.Find("Room").transform.Find("teevee").transform.Find("A").
            transform.Find("ExplationImage").GetComponent<MonitorView>();
        _ActiveObject = new GameObject[GameObject.Find("TutorialAssembly").transform.childCount];
        for (int i = 0; i < _ActiveObject.Length; i++)
        {
            _ActiveObject[i] = GameObject.Find("TutorialAssembly").transform.GetChild(i).gameObject;
            _ActiveObject[i].gameObject.SetActive(false);
        }
        PumpBody = GameObject.Find("Pump_Assembly").gameObject;
        PumpBody.SetActive(false);
        _PointC.SetLocked(true);
    }
    private void Update()
    {
        if(_Composition.IsDetechObject.Equals(true))
        {
            if (_Menu.stateDown)
            {
                ControllerHitHide(_Menu);
            }
        }
    }
    /// <summary>
    /// 텔레포트
    /// </summary>
    /// <returns></returns>
    private IEnumerator TeleportLogic()
    {
        yield return new WaitForSeconds(1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_1 , out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Teloprt]);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_2, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_3, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_4, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        ControllerHitShow(teleport, "텔레포트");
        _PointA.SetLocked(false);
        while (true)
        {
            if (_Teleport._Test != null)
            {
                var point = (_Teleport._Test as TeleportPoint);
                if (_PointA.title.Equals(point.title))
                {
                    _PointA.SetLocked(true);
                    StartCoroutine(ReturnB());
                    yield break;
                }
            }
            yield return null;
        }
    }
    IEnumerator ReturnB()
    {
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_5, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _PointB.SetLocked(false);
        while (true)
        {
            if (_Teleport._Test != null)
            {
                var point = (_Teleport._Test as TeleportPoint);
                if (_PointB.title.Equals(point.title))
                {
                    ControllerHitHide(teleport);
                    _PointB.SetLocked(true);
                    StartCoroutine(PickupObject());
                    yield break;
                }
            }
            yield return null;
        }
    }
    /// <summary>
    /// 오브젝트 잡기
    /// </summary>
    /// <returns></returns>
    private IEnumerator PickupObject()
    {
        yield return new WaitForSeconds(0.1f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.TriggerObject]);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_6, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        ControllerHitShow(_Trigger, "트리거");
        ActiveTestObject((int)StageIndex.TriggerObject);
        while (true)// 부품을 잡았는지에 대한 체크
        {
            if (Player.instance.leftHand.currentAttachedObject != null)
            {
                var data = Player.instance.leftHand.currentAttachedObject.GetComponent<AssemblyPart>();

                if (data != null)
                    break;
            }
            else if (Player.instance.rightHand.currentAttachedObject != null)
            {
                var data = Player.instance.rightHand.currentAttachedObject.GetComponent<AssemblyPart>();

                if (data != null)
                    break;
            }

            yield return null;
        }
        StartCoroutine(ToolExplantion());
    }
    /// <summary>
    /// 공구 잡기
    /// </summary>
    /// <returns></returns>
    IEnumerator ToolExplantion()
    {
        yield return new WaitForSeconds(0.1f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Tool]);
        ControllerHitHide(_Trigger);
        yield return new WaitForSeconds(1.0f);
        ActiveTestObject((int)StageIndex.Tool);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_7, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        ControllerHitShow(_Grab, "그랩");
        while (true) //공구 잡는 루프문
        {
            if (Player.instance.leftHand.currentAttachedObject != null)
            {
                var data = Player.instance.leftHand.currentAttachedObject.GetComponent<Tool>();

                if (data != null)
                    break;
            }
            else if (Player.instance.rightHand.currentAttachedObject != null)
            {
                var data = Player.instance.rightHand.currentAttachedObject.GetComponent<Tool>();

                if (data != null)
                    break;
            }

            yield return null;
        }
        StartCoroutine(Assembly());
    }
    /// <summary>
    /// 조립
    /// </summary>
    /// <returns></returns>
    IEnumerator Assembly()
    {
        yield return new WaitForSeconds(0.1f);
        PumpBody.SetActive(true);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Assembly]);
        yield return new WaitForSeconds(1.0f);
        ControllerHitHide(_Grab);
        _ActiveObject[(int)StageIndex.Assembly].gameObject.SetActive(true);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_8, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_9, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        //while (_AssemblyObject.Processivity < 100.0f) // 볼트 조립 완료 확인
        //{
        //    yield return null;
        //}
        while (!_AssemblyObject._TutorialCheck)
        {
            yield return null;
        }
        StartCoroutine(Composition());
    }
    /// <summary>
    /// 분해
    /// </summary>
    /// <returns></returns>
    IEnumerator Composition()
    {
        yield return new WaitForSeconds(0.1f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Compositon]);
        yield return new WaitForSeconds(1.0f);
        ActiveTestObject((int)StageIndex.Compositon);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_13, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_14, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        while (_Composition.GetComponent<DecompositionPartObject>().IsDetechObject == false)
        {
            yield return null;
        }
        StartCoroutine(MenuButtonExplantion());
    }
    /// <summary>
    /// 메뉴버튼
    /// </summary>
    /// <returns></returns>
    IEnumerator MenuButtonExplantion()
    {
        yield return new WaitForSeconds(0.1f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Menu]);
        yield return new WaitForSeconds(1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_10, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        ControllerHitShow(_Menu, "메뉴");
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_11, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.S_12, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _monitorView.SetMonitorImageChange(_TutorialImages[(int)StageIndex.Clear]);
        _PointC.SetLocked(false);
    }
    void HintHide(SteamVR_Action_Boolean action)
    {
        if (action.stateDown)
        {
            ControllerHitHide(action);
        }
    }
    private void ActiveTestObject(int _num)// 물체 잡기
    {
        _ActiveObject[_num].gameObject.SetActive(true);
        _ActiveObject[_num - 1].gameObject.SetActive(false);
    }
    private void ControllerHitShow(SteamVR_Action_Boolean action, string actionText)
    {
        ControllerButtonHints.ShowTextHint(Player.instance.leftHand, action, actionText);
        ControllerButtonHints.ShowTextHint(Player.instance.rightHand, action, actionText);
    }

    private void ControllerHitHide(SteamVR_Action_Boolean action)
    {
        ControllerButtonHints.HideTextHint(Player.instance.leftHand, action);
        ControllerButtonHints.HideTextHint(Player.instance.rightHand, action);
    }
}
