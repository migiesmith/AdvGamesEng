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
        Transform playerTransform = this.enemy.player.transform;
        Transform enemyTransform = this.enemy.transform;
        Vector3 targetDir = playerTransform.position - enemyTransform.position;
        float step = this.enemy.rotationspeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(enemyTransform.forward, targetDir, step * this.enemy.aimDampener, 0.0f);
        //Debug.DrawRay(enemyTransform.position, newDir, Color.red);
        enemy.transform.rotation = Quaternion.LookRotation(newDir);


        //fire weapon
        if (enemy.ammo > 0)
        {
            enemy.FireWeapon();
        }
        else
        {
            enemy.ToFlee();
        }

        rend.material.SetColor("_Color", Color.red);

    }
}
