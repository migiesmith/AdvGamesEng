/*
 When enemy has been alerted to the players presence, it will attempt to find the player
 */


using UnityEngine;
using System.Collections;

public class AlertBehaviour : Behaviour {

	private Enemy enemy;

    private Renderer rend;

	float rotationleft=0;
	

    public bool active = true;

    public float detectionAngle;
    private float range;


    private Vector3 playerPosition;
    private Vector3 enemyPosition;
    private Vector3 forward;
    private Vector3 direction;

	//empty constructor
	public AlertBehaviour(){
		
	}

	public AlertBehaviour(Enemy e){
		this.enemy = e;
        this.rend = this.enemy.indicator.GetComponent<Renderer>();
        this.range = enemy.detectionRange;
        detectionAngle = 60.0f;
    }

	public void SetRotation(float angle){
		this.rotationleft = angle;
	}

	public void update(){

		//rotate 360 degrees to search for player
		float rotation=this.enemy.rotationspeed*Time.deltaTime;
		if (rotationleft > rotation){
			rotationleft-=rotation;

            this.playerPosition = this.enemy.player.transform.position;
            this.enemyPosition = this.enemy.transform.position;

            this.forward = this.enemy.transform.forward;
            this.direction = playerPosition - enemyPosition;

            forward.Normalize();
            direction.Normalize();
            float angle = Vector3.Angle(forward, direction);
            
            //if player is in range and within the field of vision, swith to combat
            float distance = Vector3.Distance(playerPosition, enemyPosition);
            if(distance < range)
            {
                if(angle < detectionAngle/2.0f)
                {
                    this.enemy.ToCombat();
                }
            }

            //show cone of vision
            var debugLine1 = Quaternion.AngleAxis(detectionAngle/2.0f, this.enemy.transform.up) * this.enemy.transform.forward;
            var debugLine2 = Quaternion.AngleAxis((360.0f - detectionAngle/2.0f), this.enemy.transform.up) * this.enemy.transform.forward;
            Debug.DrawRay(enemy.transform.position, debugLine1 * range, Color.red);
            Debug.DrawRay(enemy.transform.position, debugLine2 * range, Color.red);

        }
		else{
			rotation=rotationleft;
			rotationleft=0;
            active = false;
			enemy.ToPatrol ();
		}

		enemy.transform.Rotate(0,rotation,0);

        if(rend!=null)
            rend.material.SetColor("_Color", Color.yellow);
    }
}
