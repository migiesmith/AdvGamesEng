using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {


	public int points = 9;
	public float dist = 1.0f;

	private LineRenderer lineRend;

	// Use this for initialization
	void Start () {
		lineRend = this.GetComponent<LineRenderer>();
		lineRend.numPositions = points;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = new Vector3(4.0f*(Mathf.Sin(Time.time) + 1.0f), Physics2D.gravity.magnitude/2.0f,0);
		float velocity = Mathf.Sqrt((vel.x * vel.x) + (vel.y * vel.y));
        float angle = Mathf.Rad2Deg*(Mathf.Atan2(vel.y , vel.x));
        float fTime = 0.2f;
        
		lineRend.SetPosition(0, new Vector3(0,0,0));
        for (int i = 1 ; i < points; i++) {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(dx , dy, 0);

			lineRend.SetPosition(i, pos);
            fTime += 1.0f / points;
        }

	}
}
