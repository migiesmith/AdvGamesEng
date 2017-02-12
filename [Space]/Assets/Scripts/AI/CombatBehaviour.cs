/*
 When the enemy has found the player, it will attack
 */ 


using UnityEngine;
using System.Collections;

public class CombatBehaviour : Behaviour {

	private Enemy enemy;

	public CombatBehaviour()
    {

	}

	public CombatBehaviour(Enemy e){
		this.enemy = e;
	}

	// Update is called once per frame
	public void update ()
    {
        if(enemy.weapon.ammo > 0)
        {
            enemy.weapon.fire();
        }
        else
        {
            enemy.ToFlee();
        }
        
	}
}
