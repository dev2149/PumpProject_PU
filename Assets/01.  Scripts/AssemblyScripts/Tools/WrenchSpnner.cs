using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class WrenchSpnner : Tool
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            _AttachObject = other.GetComponentInParent<AssemblyPart>();

            _IsAssemblyPartColliderIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AssemblyPartToolCheckCollider"))
        {
            _AttachObject = null;

            _IsAssemblyPartColliderIn = false;
        }
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        base.HandAttachedUpdate(hand);
    }

    protected override void OnAttachedToHand(Hand attachedHand)
    {
        base.OnAttachedToHand(attachedHand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);
    }

    protected override void OnHandFocusLost(Hand hand)
    {
        base.OnHandFocusLost(hand);
    }
}