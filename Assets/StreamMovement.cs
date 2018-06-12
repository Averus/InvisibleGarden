using UnityEngine;
using System.Collections;

public class StreamMovement : MonoBehaviour {


    GameObject player;
    public float origin_x;

    public float z;
    public float x;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Main");
        origin_x = transform.position.x;
	
	}
	
	// Update is called once per frame
	void Update () {


        if (player.transform.position.z > 40)
        {
            Vector3 v = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = (v);
        }

        if (player.transform.position.z > 30 && player.transform.position.z <= 40)
        {
            z = (40 - player.transform.position.z);

            x = z * (z*0.05f) + origin_x;

            Vector3 v = new Vector3(x, transform.position.y, player.transform.position.z);
            transform.position = (v);

        }

        if (player.transform.position.z > 15 && player.transform.position.z <= 30)
        {
            z = (30 - player.transform.position.z);

            x = 5 - z * (z * 0.04f) + origin_x;

            Vector3 v = new Vector3(x, transform.position.y, player.transform.position.z);
            transform.position = (v);
        }

        if (player.transform.position.z > 10 && player.transform.position.z <= 15)
        {
            z = (15 - player.transform.position.z);

            x = -4 + z * (z * 0.04f) + origin_x;

            Vector3 v = new Vector3(x, transform.position.y, player.transform.position.z);
            transform.position = (v);
        }

        if (player.transform.position.z > 0 && player.transform.position.z <= 10)
        {
   
        }













    }
}
