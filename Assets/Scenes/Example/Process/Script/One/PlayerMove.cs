using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public bool IsNeedToMove = false;

    private Vector3 MovePosition;

    private float time = 0;

    [HideInInspector]
    public bool IsComplete = false;

	// Use this for initialization
	void Start () {
        MovePosition = this.transform.position + transform.forward * 3;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsNeedToMove) {
            IsComplete = false;
            this.transform.position = Vector3.Lerp(this.transform.position, MovePosition, time/10);
            time += Time.deltaTime;
            if (time > 3) {
                time = 0;
                IsNeedToMove = false;
                IsComplete = true;
            }
           
        }
            
	}
}
