/*
 When the enemy has found the player, it will attack
 */ 


using UnityEngine;
using System.Collections;

public class CombatBehaviour : Behaviour {

	private Enemy enemy;

    private Renderer rend;

    public CombatBehaviour()
    {

	}

	public CombatBehaviour(Enemy e){
		this.enemy = e;

        this.rend = this.enemy.indicator.GetComponent<Renderer>();
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

        rend.material.SetColor("_Color", Color.red);

    }
}
