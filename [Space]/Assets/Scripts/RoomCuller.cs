﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start()
    {
        this.currUpdateTime = this.updateTime;
        this.lastForward = this.forwardToTrack.forward;
    }

    // Update is called once per frame
    void Update()
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
            if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit))
            {
                // Update what room we are in
                RoomBehaviour room = hit.transform.gameObject.GetComponent<RoomBehaviour>();
                if (room != null)
                {
                    if (this.currentRoom != null)
                    {
                        cullFromRoom(currentRoom, false);
                    }
                    this.currentRoom = room;
                    cullFromRoom(currentRoom, true);
                    this.lastForward = this.forwardToTrack.forward;
                }
            }

            // Reset room update timer
            this.currUpdateTime = this.updateTime;
        }
        // Decrement our timer
        currUpdateTime -= Time.deltaTime;
    }

	// Toggles rooms around roomBehaviour
    public void cullFromRoom(RoomBehaviour roomBehaviour, bool isActive)
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
                    //float dot = Vector3.Dot(Vector3.Normalize(next.connections[i].connectedRoom.position - this.currentRoom.room.position), forward);
                    //if(dot >= -0.6f){
                    	toSee.Add(next.connections[i].connectedRoom);
                    //}
                    depth++;
                }
            }
        }

		// Loop through each room in seen and set its active state to the boolean isActive
        foreach (Room r in seen)
        {
            RoomBehaviour rB = r.getRoomBehaviour();
            for (int i = 0; i < rB.transform.childCount; i++)
            {
                rB.transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }

    }

}
