using UnityEngine;
using System.Collections;

public class SwitchCameras : MonoBehaviour {


    Camera mainCamera;
    Camera playerCamera;

	// Use this for initialization
	void Start () {

        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>().GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerAvatar").GetComponent<Camera>().GetComponent<Camera>();

        mainCamera.enabled = true;
        playerCamera.enabled = false;

    }


    public void Switch()
    {
        mainCamera.enabled = !mainCamera.enabled;
        playerCamera.enabled = !playerCamera.enabled;
    }

    // Update is called once per frame
    void Update () {

     
	
	}
}
