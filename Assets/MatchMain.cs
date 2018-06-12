using UnityEngine;
using System.Collections;

public class MatchMain : MonoBehaviour {

    GameObject main;

	// Use this for initialization
	void Start () {

        main = GameObject.Find("Main");
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(main.transform.position.x, transform.position.y, main.transform.position.z);

        transform.localEulerAngles = new Vector3(90f, main.transform.eulerAngles.y, transform.rotation.z);
    }
}
