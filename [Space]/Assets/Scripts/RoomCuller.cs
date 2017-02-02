using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCuller : MonoBehaviour {

	private RoomBehaviour currentRoom;
	
	public Transform forwardToTrack;
	private Vector3 lastForward;

	public int cullRange = 20;

	public float updateTime = 1.0f;
	public float currUpdateTime = 0.0f;

	public float forceUpdateDistance = 3.0f;
	public float forceUpdateAngle = 90.0f;

	// Use this for initialization
	void Start () {
		this.currUpdateTime = this.updateTime;
		this.lastForward = this.forwardToTrack.forward;
	}
	
	// Update is called once per frame
	void Update () {

		if(Vector3.Angle(this.lastForward, this.forwardToTrack.forward) >= forceUpdateAngle
			|| this.currentRoom && Vector3.Distance(this.currentRoom.transform.position, this.forwardToTrack.position) >= forceUpdateDistance){
			
			currUpdateTime = 0.0f;

		}

		// Check if we should update the current room
		if(currUpdateTime <= 0.0f){
			// Check what room we are in
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit)){
				// Update what room we are in
				RoomBehaviour room = hit.transform.gameObject.GetComponent<RoomBehaviour>();
				if(room != null){
					if(this.currentRoom != null){
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
		currUpdateTime -= Time.deltaTime;

	}

	public void cullFromRoom(RoomBehaviour roomBehaviour, bool isActive){
		
		Vector3 forward = isActive ? forwardToTrack.forward : lastForward;
    	List<Room> seen = new List<Room>();
        List<Room> toSee = new List<Room>();
		int depth = 0;
        toSee.Add(roomBehaviour.room);
        while (toSee.Count > 0 && depth < cullRange) {
            Room next = toSee[0];
            toSee.RemoveAt(0);
            for (int i = 0; i < next.connections.Length; i++)
            {
                if (next.connections[i].connectedRoom != null && !seen.Contains(next.connections[i].connectedRoom)) {
					float dot = Vector3.Dot(Vector3.Normalize(next.connections[i].connectedRoom.position - this.currentRoom.room.position), forward);
					//if(dot >= -0.4f){
						toSee.Add(next.connections[i].connectedRoom);
					//}
					depth++;
                }
            }			

            seen.Add(next);
        }
			
		foreach(Room r in seen){
			RoomBehaviour rB = r.getRoomBehaviour();
			for(int i = 0; i < rB.transform.childCount; i++){
				rB.transform.GetChild(i).gameObject.SetActive(isActive);
			}
		}

	}

}
