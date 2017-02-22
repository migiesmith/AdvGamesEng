using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlider : MonoBehaviour
{

    // Curve followed when opening
    public AnimationCurve openCurve = AnimationCurve.EaseInOut(0,0,1,1);
	// Time it takes for the door to open 
	public float openingDuration = 2.0f;
    // Curve followed when closing
	public AnimationCurve closeCurve = AnimationCurve.EaseInOut(1,1,0,0);
	// Time it takes for the door to close 
	public float closingDuration = 2.0f;

	// The position of the closed door (set in the start method)
    private Vector3 closedPos;
	// The offset when open
    public Vector3 openOffset;


	// States the door can be in
    public enum DoorState
    {
        OPEN,
        OPENING,
        CLOSED,
        CLOSING
    }

	// Current state of the door
    public DoorState state = DoorState.CLOSED;

	// Progress through the opening/closing animation
    private float animProgress = 0.0f;

    // Use this for initialization
    void Start()
    {
        // Set the starting position
        this.closedPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
			if(state == DoorState.CLOSED || state == DoorState.CLOSING){
				open();
			}else{
				close();
			}
        }
		// Check what state we are in
        switch (state)
        {
            case DoorState.OPEN:
                {
					// Set the door's position
					this.transform.position = closedPos + openOffset;
					// Set the animation progress to 0
                    animProgress = 0.0f;
                }
                break;
            case DoorState.OPENING:
                {
					// Check the lerp amount from the curve
                    float curveVal = openCurve.Evaluate(animProgress / openingDuration);
					// Apply the lerp
					this.transform.position = Vector3.Lerp(closedPos, closedPos + openOffset, curveVal);
					// Increment the animation progress
                    animProgress += Time.deltaTime;
					// Check if the animation has finished
					if(animProgress >= openingDuration){
						// Change state and reset the animation progress
                    	animProgress = 0.0f;
						state = DoorState.OPEN;
					}
                }
                break;
            case DoorState.CLOSED:
                {
					// Set the door's position
					this.transform.position = closedPos;
					// Set the animation progress to 0
                    animProgress = 0.0f;
                }
                break;
            case DoorState.CLOSING:
                {
					// Check the lerp amount from the curve
                    float curveVal = closeCurve.Evaluate(animProgress / closingDuration);
					// Apply the lerp
					this.transform.position = Vector3.Lerp(closedPos + openOffset, closedPos, curveVal);
					// Increment the animation progress
                    animProgress += Time.deltaTime;
					// Check if the animation has finished
					if(animProgress >= closingDuration){
						// Change state and reset the animation progress
						state = DoorState.CLOSED;
                    	animProgress = 0.0f;
					}
                }
                break;

        }
    }

	// Returns the state of the door
	public DoorState getState(){
		return state;
	}

	// Opens the door
    public void open()
    {
		// If the door is closing then flip the animation progress
		if(state == DoorState.CLOSING)
			animProgress = openingDuration - openingDuration * animProgress/closingDuration;
		// Change state
		state = DoorState.OPENING;
    }

	// Closes the door
    public void close()
    {
		// If the door is opening then flip the animation progress
		if(state == DoorState.OPENING)
			animProgress = closingDuration - closingDuration * animProgress/openingDuration;
		// Change state
		state = DoorState.CLOSING;
    }


}
