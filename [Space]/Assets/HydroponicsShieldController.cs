using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroponicsShieldController : MonoBehaviour {

	Renderer rend;
	private Vector2 offset;
	public Vector2 offsetSpeed = new Vector2(0.001f, 0.001f);

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(rend != null)
		{
			offset += offsetSpeed * Time.deltaTime;
			rend.material.SetTextureOffset("_BumpMap", offset);
		}
	}
}
