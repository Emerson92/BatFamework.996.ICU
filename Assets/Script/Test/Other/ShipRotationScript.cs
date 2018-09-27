using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.position,this.transform.up,0.5f);
	}
}
