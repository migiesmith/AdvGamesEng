/*
 When the enemy has taken damage, it will attempt to escape
 */ 

using UnityEngine;
using System.Collections;

public class FleeBehaviour : Behaviour {

	private Enemy enemy;

	public FleeBehaviour(){

	}

	public FleeBehaviour(Enemy e){
		this.enemy = e;
	}

	public void update(){
		
	}
}
