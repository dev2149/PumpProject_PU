using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Tool : MonoBehaviour
{
    public enum Type
    {
        None = -1,
        HexM6,
        HexM8,  //13
        HexM10, //17
        HexM12, //19
        HexM14, //22
        HexM16,
        HexM18,
        HexM20,
        LenchM8,
        LenchM10,
        LenchM12,
        HexM24,
        HexM27,
    }

    public enum ToolType
    { 
        None,
        HandTool,
        ElecTool
    }


    [SerializeField] protected GameObject[] _HandObject;
    [SerializeField] protected SteamVR_Action_Boolean _ToolUseAction;
    [SerializeField] protected Type _Type;
    [SerializeField] protected ToolType _ToolType;

    protected Hand _Hand;

    protected bool _IsTriggerUp;

    protected bool _IsTriggerDown;
    protected bool _IsTriggerDownChk;

    protected bool _IsHaptic;

    protected bool _IsAssemblyPartColliderIn;

    protected bool _InsertCheck;

    protected AssemblyPart _AttachObject;
    protected Coroutine _HapticRoutine;

    /// <summary>
    /// Hand에서 호출
    /// </summary>
    /// <param name="hand"></param>
    virtual protected void HandAttachedUpdate(Hand hand)
    {

    }

    /// <summary>
    /// Hand에서 호출
    /// </summary>
    /// <param name="attachedHand"></param>
    virtual protected void OnAttachedToHand(Hand attachedHand)
    {
        _Hand = attachedHand;

        //Director.Instance.SoundManager.PlayEffectSound(SoundManager.EffectSoundIndex.ObjectGrap);

        _ToolUseAction.AddOnStateDownListener(TriggerDown, attachedHand.handType);
        _ToolUseAction.AddOnStateUpListener(TriggerUp, attachedHand.handType);

        if (_HandObject.Length > 0)
        {
            if (_Hand.handType.Equals(SteamVR_Input_Sources.LeftHand))
                _HandObject[0].SetActive(true);
            else if (_Hand.handType.Equals(SteamVR_Input_Sources.RightHand))
                _HandObject[1].SetActive(true);
        }
    }

    /// <summary>
    /// Hand에서 호출
    /// </summary>
    /// <param name="hand"></param>
    virtual protected void OnDetachedFromHand(Hand hand)
    {
        _ToolUseAction.RemoveOnStateDownListener(TriggerDown, hand.handType);
        _ToolUseAction.RemoveOnStateUpListener(TriggerUp, hand.handType);

        if (_HandObject.Length > 0)
        {
            if (hand.handType.Equals(SteamVR_Input_Sources.LeftHand))
                _HandObject[0].SetActive(false);
            else if (hand.handType.Equals(SteamVR_Input_Sources.RightHand))
                _HandObject[1].SetActive(false);
        }

        hand = null;
    }

    /// <summary>
    /// Hand에서 호출
    /// </summary>
    /// <param name="hand"></param>
    virtual protected void OnHandFocusLost(Hand hand)
    {
        gameObject.SetActive(false);
    }

    protected IEnumerator HapticRoutine()
    {
        while (true)
        {
            _Hand.TriggerHapticPulse(1500);
            yield return new WaitForSeconds(0.05f);

            _Hand.TriggerHapticPulse(800);
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected IEnumerator HapticRoutine2()
    {
        while (true)
        {
            _Hand.TriggerHapticPulse(1500);
            yield return new WaitForSeconds(0.05f);

            _Hand.TriggerHapticPulse(1500);
            yield return new WaitForSeconds(0.05f);

            _Hand.TriggerHapticPulse(1500);
            yield return new WaitForSeconds(0.05f);

            _Hand.TriggerHapticPulse(1500);
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected void TriggerUp(SteamVR_Action_Boolean forAction, SteamVR_Input_Sources formSource)
    {
        _IsTriggerUp = true;
        _IsTriggerDown = false;
    }

    protected void TriggerDown(SteamVR_Action_Boolean forAction, SteamVR_Input_Sources formSource)
    {
        _IsTriggerDown = true;
        _IsTriggerUp = false;
    }
}