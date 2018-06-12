using UnityEngine;
using System.Collections;

public class TiltToLook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Input.acceleration.x, 0, 0);
	
	}
}
