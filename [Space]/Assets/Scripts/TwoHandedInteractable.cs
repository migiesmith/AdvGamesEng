using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class TwoHandedInteractable : NVRInteractable
{

    public NVRHand SecondAttachedHand;

    protected NVRHand rHand, lHand;


    public override bool IsAttached
    {
        get
        {
            return AttachedHand != null || SecondAttachedHand != null;
        }
    }


	protected override void Start(){
		base.Start();
	}

    protected void LateStart(){
        rHand = GameObject.Find("RightHand").GetComponent<NVRHand>();
        lHand = GameObject.Find("LeftHand").GetComponent<NVRHand>();
    }

    public override void ResetInteractable()
    {
        base.ResetInteractable();
        SecondAttachedHand = null;
    }

    protected override bool CheckForDrop()
    {
        float shortestDistance = float.MaxValue;

        NVRHand mainHand = (AttachedHand != null ? AttachedHand : SecondAttachedHand);

        for (int index = 0; index < Colliders.Length; index++)
        {
            //todo: this does not do what I think it does.
            Vector3 closest = Colliders[index].ClosestPointOnBounds(mainHand.transform.position);
            float distance = Vector3.Distance(mainHand.transform.position, closest);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                ClosestHeldPoint = closest;
            }
        }

        if (DropDistance != -1 && mainHand.CurrentInteractionStyle != InterationStyle.ByScript && shortestDistance > DropDistance)
        {
            DroppedBecauseOfDistance();
            return true;
        }

        return false;
    }

    protected override void Update()
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

    public override void BeginInteraction(NVRHand hand)
    {
        if (AttachedHand != null)
        {
            SecondAttachedHand = hand;
            if (DisableKinematicOnAttach == true)
            {
                Rigidbody.isKinematic = false;
            }
        }
        else
        {
            base.BeginInteraction(hand);
        }
    }

    public override void InteractingUpdate(NVRHand hand)
    {
        if (AttachedHand == hand)
        {
            base.InteractingUpdate(hand);
        }
        else
        {
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

    public override void EndInteraction()
    {
        
        if(SecondAttachedHand != null && SecondAttachedHand.CurrentlyInteracting != this)
        {            
                SecondAttachedHand = null;
        }
        if( AttachedHand != null && AttachedHand.CurrentlyInteracting != this)
        {            
                AttachedHand = null;
        }

		if(AttachedHand == null && SecondAttachedHand == null)
		{
			base.EndInteraction();
		}
    }

    protected override void DroppedBecauseOfDistance()
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
