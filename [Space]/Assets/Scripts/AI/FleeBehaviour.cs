/*
 When the enemy has taken damage, it will attempt to escape
 */ 

using UnityEngine;
using System.Collections;

public class FleeBehaviour : Behaviour {

	private Enemy enemy;

    private Renderer rend;

	public FleeBehaviour(){

	}

	public FleeBehaviour(Enemy e){
		this.enemy = e;

        this.rend = this.enemy.indicator.GetComponent<Renderer>();
    }

	public void update(){
        rend.material.SetColor("_Color", Color.blue);
    }
}
