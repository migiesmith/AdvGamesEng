/*
 When the enemy has taken damage, it will attempt to escape
 */ 

using UnityEngine;
using System.Collections;

public class FleeBehaviour : Behaviour {

	private GameEnemy enemy;

    private Renderer rend;

	public FleeBehaviour(){

	}

	public FleeBehaviour(GameEnemy e){
		this.enemy = e;

        this.rend = this.enemy.indicator.GetComponent<Renderer>();
    }

	public void update(){
        rend.material.SetColor("_Color", Color.blue);
        this.enemy.die();
    }
}
