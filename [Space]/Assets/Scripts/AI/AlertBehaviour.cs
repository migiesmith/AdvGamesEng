/*
 When enemy has been alerted to the players presence, it will attempt to find the player
 */


using UnityEngine;
using System.Collections;

public class AlertBehaviour : Behaviour {

	private GameEnemy enemy;

    private Renderer rend;

	float rotationleft=0;
	
    

    public float detectionAngle;
    private float range;


    private Vector3 playerPosition;
    private Vector3 enemyPosition;
    private Vector3 forward;
    private Vector3 direction;

    bool finishedRotation = false;
    bool finishedTraversal = false;
    bool finishedLeft = false;
    bool finishedRight = false;

    Vector3 left = new Vector3();
    Vector3 right = new Vector3();


    float angleDistance = 0.0f;

    //empty constructor
    public AlertBehaviour(){
		
	}

	public AlertBehaviour(GameEnemy e){
		this.enemy = e;
        this.rend = this.enemy.indicator.GetComponent<Renderer>();
        this.range = enemy.detectionRange;
        detectionAngle = 60.0f;
    }

	public void SetRotation(float angle){
		this.rotationleft = angle;
	}

	public void update(){
        /*
		//rotate 360 degrees to search for player
		float rotation=this.enemy.rotationspeed*Time.deltaTime;
		if (rotationleft > rotation && this.enemy.alertActive){
			rotationleft-=rotation;
            */
            this.playerPosition = this.enemy.player.transform.position;
            this.enemyPosition = this.enemy.transform.position;

            this.forward = this.enemy.transform.forward;
            this.direction = playerPosition - enemyPosition;

            forward.Normalize();
            direction.Normalize();
            float angle = Vector3.Angle(forward, direction);
            
            //if player is in range and within the field of vision, swith to combat
            float distance = Vector3.Distance(playerPosition, enemyPosition);
            if(distance < range && angle < detectionAngle / 2.0f && this.enemy.checkLineOfSight())
            {
                //Debug.Log(angle);
                
                //reset flags
                this.enemy.alertActive = false;
                this.finishedLeft = false;
                this.finishedRight = false;
                this.finishedRotation = false;
                this.finishedTraversal = false;

                this.enemy.ToCombat();
                
            }

            //show cone of vision
            var debugLine1 = Quaternion.AngleAxis(detectionAngle/2.0f, this.enemy.transform.up) * this.enemy.transform.forward;
            var debugLine2 = Quaternion.AngleAxis((360.0f - detectionAngle/2.0f), this.enemy.transform.up) * this.enemy.transform.forward;
            Debug.DrawRay(enemy.transform.position, debugLine1 * range, Color.red);
            Debug.DrawRay(enemy.transform.position, debugLine2 * range, Color.red);

        
        //aim towards player
        Transform enemyTransform = this.enemy.transform;
        Vector3 targetDir = this.enemy.lastKnownLocation - enemyTransform.position;
        if(angleDistance == 0.0f)
            angleDistance = Vector3.Angle(enemy.transform.forward, targetDir);

        
        //rotate towards enemy
        if (!finishedRotation)
        {
            float angleTravelled = Vector3.Angle(enemy.transform.forward, targetDir);
            float rotationSpeed = enemy.inertia.Evaluate((angleDistance - angleTravelled) / angleDistance);
            
            float step = this.enemy.rotationspeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(enemyTransform.forward, targetDir, (step * this.enemy.aimDampener * rotationSpeed), 0.0f);
            //Debug.DrawRay(enemyTransform.position, newDir, Color.red);
            enemy.transform.rotation = Quaternion.LookRotation(newDir);
            if (Vector3.Angle(enemyTransform.forward, targetDir) < 5)
            {
                finishedRotation = true;
            }
        }
        //move towards last known location
        else if(!finishedTraversal)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.lastKnownLocation, 5.0f * Time.deltaTime);
            left = Quaternion.AngleAxis(90.0f, this.enemy.transform.up) * this.enemy.transform.forward;
            right = Quaternion.AngleAxis(-90, this.enemy.transform.up) * this.enemy.transform.forward;
            if (Vector3.Distance(enemy.transform.position, enemy.lastKnownLocation) < 0.5f)
            {
                finishedTraversal = true;
                angleDistance = Vector3.Angle(enemy.transform.forward, left);
            }
        }

        //look left
        else if(!finishedLeft)
        {
            float rotationSpeed = enemy.inertia.Evaluate((angleDistance - Vector3.Angle(enemy.transform.forward, left)) / angleDistance);
            Vector3 newAngle = Vector3.RotateTowards(this.enemy.transform.forward, left, (rotationSpeed * Time.deltaTime * this.enemy.aimDampener * this.enemy.rotationspeed), 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newAngle);

            if (Vector3.Angle(enemyTransform.forward, left) < 5)
            {
                finishedLeft = true;
                angleDistance = Vector3.Angle(enemy.transform.forward, right);
            }

        }
        //look right
        else if (!finishedRight)
        {
            float rotationSpeed = enemy.inertia.Evaluate((angleDistance - Vector3.Angle(enemy.transform.forward, right)) / angleDistance);
            Vector3 newAngle = Vector3.RotateTowards(this.enemy.transform.forward, right, (rotationSpeed * Time.deltaTime * this.enemy.aimDampener * this.enemy.rotationspeed), 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newAngle);

            if (Vector3.Angle(enemyTransform.forward, right) < 5)
            {
                finishedRight = true;
            }
        }
        //resume patrol
        else
        {
            //reset flags
            this.enemy.alertActive = false;
            this.finishedLeft = false;
            this.finishedRight = false;
            this.finishedRotation = false;
            this.finishedTraversal = false;

            //switch active behaviour to patrol
            this.enemy.ToPatrol();
            
        }

        if (rend!=null)
            rend.material.SetColor("_Color", Color.yellow);
    }
}
