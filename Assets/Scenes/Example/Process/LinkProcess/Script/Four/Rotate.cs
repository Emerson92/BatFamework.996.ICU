using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public bool IsNeedRotate = true;

    private Vector3 axis;

	// Use this for initialization
	void Start () {
        axis = this.transform.InverseTransformDirection(Vector3.up);

    }
	
	// Update is called once per frame
	void Update () {
        if (IsNeedRotate)
            this.transform.Rotate(axis, 1f);
	}
}
