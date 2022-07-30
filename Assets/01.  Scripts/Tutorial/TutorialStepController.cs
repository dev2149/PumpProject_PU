using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class TutorialStepController : MonoBehaviour
{
    [SerializeField] Teleport _teleport;
    [SerializeField] GameObject _PlayerObj;
    [SerializeField] TutorialHint _ControllerHint;
    #region StepList
    [HideInInspector] public SteamVR_Action_Boolean menuAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("MenuUI");
    [HideInInspector] public SteamVR_Action_Boolean uiInteractAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    [HideInInspector] public SteamVR_Action_Boolean grabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    [SerializeField] GameObject[] _StepObj;
    #endregion
    private void Start()
    {
        _teleport = GameObject.Find("Teleporting").GetComponent<Teleport>();
        _PlayerObj = GameObject.Find("Player").gameObject;
        _ControllerHint = GetComponent<TutorialHint>();
        _StepObj = new GameObject[this.gameObject.transform.childCount];
        for (int i = 0; i < _StepObj.Length; i++)
        {
            _StepObj[i] = this.gameObject.transform.GetChild(i).gameObject;
            _StepObj[i].SetActive(false);
        }
        _StepObj[0].SetActive(true);
    }
    public void NextStepCoroutine(int _num)
    {
        StartCoroutine(NextStepFadeInOut(_num));
    }
    public IEnumerator NextStepFadeInOut(int _num)
    {
        SteamVR_Fade.View(Color.black, 1);
        SteamVR_Fade.Start(Color.black, 1);
        yield return new WaitForSeconds(1f);
        DivisionStep(_num);
        SteamVR_Fade.View(Color.clear, 1);
        SteamVR_Fade.Start(Color.clear, 1);
    }
    void DivisionStep(int _num)
    {
        switch (_num)
        {
            case 1:
                _teleport.ShowTeleportHint();
                NextStep(_num);
                break;
            case 2:
                _teleport.CancelTeleportHint();
                _ControllerHint.ShowMenuButtonHint(menuAction, true, "Menu");
                NextStep(_num);
                break;
            case 3:
                _ControllerHint.ShowMenuButtonHint(menuAction, false);
                _ControllerHint.ShowMenuButtonHint(uiInteractAction, false);
                NextStep(_num , 1.2f , -0.45f , 180.0f);
                break;
        }
    }
    void NextStep(int _num,float _posX = 0.0f , float _posZ = 0.0f, float _Rot = 0.0f)
    {
        _PlayerObj.transform.position = new Vector3(_posX, _PlayerObj.transform.position.y,
            _posZ);
        _PlayerObj.transform.rotation = new Quaternion(_PlayerObj.transform.rotation.x,
            _Rot, _PlayerObj.transform.rotation.z, _PlayerObj.transform.rotation.w);
        if (_StepObj[_num] != null)
        {
            _StepObj[_num].SetActive(true);
            _StepObj[_num - 1].SetActive(false);
        }
    }
}