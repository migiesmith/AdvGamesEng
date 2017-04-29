/*
 When the enemy has found the player, it will attack
 */ 


using UnityEngine;
using System.Collections;

public class CombatBehaviour : Behaviour {

	private GameEnemy enemy;

    private Renderer rend;

    public CombatBehaviour()
    {

	}

	public CombatBehaviour(GameEnemy e){
		this.enemy = e;

        this.rend = this.enemy.indicator.GetComponent<Renderer>();
    }

	// Update is called once per frame
	public void update ()
    {

        //aim towards player
        Transform enemyTransform = this.enemy.transform;
        Vector3 targetDir = this.enemy.player.position - this.enemy.weaponTransform.position;
        float step = this.enemy.rotationspeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(enemyTransform.forward, targetDir, step * this.enemy.aimDampener, 0.01f);
        //Debug.DrawRay(enemyTransform.position, newDir, Color.red);
        enemy.transform.rotation = Quaternion.LookRotation(newDir);

        //enemy can see the player
        if (enemy.checkLineOfSight())
        {
            //fire weapon
            enemy.FireWeapon();
            enemy.timeSinceSeen = 0;
        }
        //delay the switch back to alert by a few seconds
        else if (enemy.timeSinceSeen >= enemy.timeToLose)
        {
            enemy.playerExitCombat();
            enemy.ToAlert();
            enemy.timeSinceSeen = 0;
        }
        else
        {
            enemy.timeSinceSeen += Time.deltaTime;
        }
        
        

        rend.material.SetColor("_Color", Color.red);

    }
}
