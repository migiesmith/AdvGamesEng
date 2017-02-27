using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class TwoHandedInteractable : NVRInteractable {

	public NVRHand SecondAttachedHand;


	new public virtual void ResetInteractable(){
		base.ResetInteractable();
		SecondAttachedHand = null;
	}

	new protected virtual void Update()
	{
		if (this.transform.position.y > 10000 || this.transform.position.y < -10000)
		{
			if (AttachedHand != null)
				AttachedHand.EndInteraction(this);

			if (SecondAttachedHand != null)
				SecondAttachedHand.EndInteraction(this);

			Destroy(this.gameObject);
		}
	}

	new public virtual void BeginInteraction(NVRHand hand){
		if(AttachedHand != null){
			SecondAttachedHand = hand;
		}else{
			base.BeginInteraction(hand);
		}
	}

	new public virtual void InteractingUpdate(NVRHand hand)
	{
		if(AttachedHand == hand){
			base.InteractingUpdate(hand);
		}else{
			if (hand.UseButtonUp == true)
			{
				UseSecondButtonUp();
			}

			if (hand.UseButtonDown == true)
			{
				UseSecondButtonDown();
			}
		}
	}

	new public void ForceDetach()
	{
		base.ForceDetach();

		if (SecondAttachedHand != null)
			SecondAttachedHand.EndInteraction(this);

		if (SecondAttachedHand != null)
			EndInteraction();
	}

	new public virtual void EndInteraction(){
		base.EndInteraction();

		AttachedHand = SecondAttachedHand;
		SecondAttachedHand = null;
	}

	new protected virtual void DroppedBecauseOfDistance()
	{
		base.DroppedBecauseOfDistance();
		SecondAttachedHand.EndInteraction(this);
	}

	public virtual void UseSecondButtonUp()
	{

	}

	public virtual void UseSecondButtonDown()
	{

	}
}
