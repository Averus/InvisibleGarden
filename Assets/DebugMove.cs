using UnityEngine;
using System.Collections;

public class DebugMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * (4*Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * (4 * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(1, 0, 0);
        }



    }
               
        
}
