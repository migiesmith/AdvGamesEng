/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CielaSpike;

public class RoomCuller : MonoBehaviour
{

    // The room the culler is in
    private RoomBehaviour currentRoom;
    // The object defining the direction the culler is facing
    public Transform forwardToTrack;
    // The direction that was last culled
    private Vector3 lastForward;

    // Max depth
    public int cullRange = 20;

    // The time between each update
    public float updateTime = 0.1f;
    // The time left till the next update
    public float currUpdateTime = 0.0f;
    // Force update if distance traveled exceeds this value
    public float forceUpdateDistance = 3.0f;
    // Force update if angle delta exceeds this value
    public float forceUpdateAngle = 90.0f;

    [Range(-1,1)]
    public float directionCutOff = -0.8f;

    // Use this for initialization
    void Start()
    {
        this.currUpdateTime = this.updateTime;
        this.lastForward = this.forwardToTrack.forward;

        RoomBehaviour[] allRooms = GameObject.FindObjectsOfType<RoomBehaviour>();
        foreach(RoomBehaviour room in allRooms)
        {
            room.hide();
        }

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("cull");
    }

    IEnumerator cull()
    {        
        // Check for a large angle or distance change
        if (Vector3.Angle(this.lastForward, this.forwardToTrack.forward) >= forceUpdateAngle
            || this.currentRoom && Vector3.Distance(this.currentRoom.transform.position, this.forwardToTrack.position) >= forceUpdateDistance)
        {

            // Force a cull update
            currUpdateTime = 0.0f;
        }

        // Check if we should update the current room
        if (currUpdateTime <= 0.0f)
        {
            // Check what room we are in
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit, 6.0f, LayerMask.NameToLayer("Sensor")))
            {
                // Update what room we are in
                RoomBehaviour room = hit.transform.gameObject.GetComponent<RoomBehaviour>();
                if (room != null)
                {
                    List<Room> toHide = new List<Room>();
                    if (this.currentRoom != null)
                    {
                        toHide = toCullFromRoom(currentRoom, false);
                    }
                    this.currentRoom = room;                    
                    List<Room> toShow = toCullFromRoom(currentRoom, true);
                    this.lastForward = this.forwardToTrack.forward;

                    yield return Ninja.JumpToUnity;
                    // Loop through each room in toShow and show it
                    foreach (Room r in toShow)
                    {
                        RoomBehaviour rB = r.getRoomBehaviour();
                        rB.show();
                        // Remove the room from the toHide list
                        toHide.Remove(r);
                    }

                    // Loop through each room in toHide and hide it
                    foreach (Room r in toHide)
                    {
                        RoomBehaviour rB = r.getRoomBehaviour();
                        rB.hide();
                    }
                }
            }

            // Reset room update timer
            this.currUpdateTime = this.updateTime;
        }
        // Decrement our timer
        currUpdateTime -= Time.deltaTime;
    }

	// Toggles rooms around roomBehaviour
    public List<Room> toCullFromRoom(RoomBehaviour roomBehaviour, bool isActive)
    {
		// Use the last forward if we are deactivating as forward likely changed
		Vector3 forward = isActive ? forwardToTrack.forward : lastForward;
        List<Room> seen = new List<Room>(); // Keep track of what we have seen
        List<Room> toSee = new List<Room>(); // Keep track of what we have yet to see

        int depth = 0; // Keep track of the travelled depth
        toSee.Add(roomBehaviour.room);
        while (toSee.Count > 0 && depth < cullRange)
        {
            Room next = toSee[0];
            toSee.RemoveAt(0);			
            seen.Add(next);
            for (int i = 0; i < next.connections.Length; i++)
            {
				// Check if we should add the current room
				if (next.connections[i].connectedRoom != null && !seen.Contains(next.connections[i].connectedRoom) && !toSee.Contains(next.connections[i].connectedRoom))
                {
                    float dot = Vector3.Dot(Vector3.Normalize(next.connections[i].connectedRoom.position - this.currentRoom.room.position), forward);
                    if(dot >= directionCutOff){
                    	toSee.Add(next.connections[i].connectedRoom);
                    }
                    depth++;
                }
            }
        }

        return seen;
    }

}
