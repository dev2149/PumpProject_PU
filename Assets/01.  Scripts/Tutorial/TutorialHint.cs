using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class TutorialHint : MonoBehaviour
{
    [SerializeField] Hand[] hands;
    [SerializeField] TutorialStepController tutorialStep;
    private void Start()
    {
        hands = new Hand[2];
        hands[0] = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("LeftHand").GetComponent<Hand>();
        hands[1] = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand").GetComponent<Hand>();
    }
    private void Update()
    {
        //MenuButtonAction();
    }
    void MenuButtonAction()
    {
        if (tutorialStep.menuAction.stateDown)
        {
            ShowMenuButtonHint(tutorialStep.menuAction, false);

            ShowMenuButtonHint(tutorialStep.uiInteractAction , true, "Trigger");
        }
        else if (tutorialStep.uiInteractAction.stateDown)
        {
            ShowMenuButtonHint(tutorialStep.uiInteractAction, false);
        }
    }
    public void ShowMenuButtonHint(SteamVR_Action_Boolean _action , bool _b , string _txt = "")// SteamVR_Action_Boolean의 변수값을 튜토리얼 컨트롤러 스크립트로 이동
    {
        if (_b)
        {
            for (int i = 0; i < hands.Length; i++)
            {
                ControllerButtonHints.ShowButtonHint(hands[i], _action);
                ControllerButtonHints.ShowTextHint(hands[i], _action, _txt);
            }
        }
        else
        {
            for (int i = 0; i < hands.Length; i++)
            {
                ControllerButtonHints.HideButtonHint(hands[i], _action);
                ControllerButtonHints.HideTextHint(hands[i], _action);
            }
        }
    }
}