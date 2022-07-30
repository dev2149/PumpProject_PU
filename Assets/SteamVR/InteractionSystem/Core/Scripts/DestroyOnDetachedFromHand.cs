//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Destroys this object when it is detached from the hand
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class DestroyOnDetachedFromHand : MonoBehaviour
	{
		[SerializeField] AudioSource _GrapSound;
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			StartCoroutine(Delay());

		}
		IEnumerator Delay()
        {
			_GrapSound.Play();
			yield return new WaitForSeconds(0.15f);
			Destroy(gameObject);
		}
	}
}
