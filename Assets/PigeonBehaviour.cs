using UnityEngine;
using System.Collections;

public class PigeonBehaviour : MonoBehaviour {


    public GameObject child;
    AudioSource audioSource;

    public Vector3 returnPosition;
    public Vector3 flightDestination;
    public float flightSpeed = 2f;
    public float flightHeight;


    public int state = 0;

	// Use this for initialization
	void Start () {

        returnPosition = transform.position;

        audioSource = gameObject.GetComponent<AudioSource>();

        
	
	}


   public void FlyAway()
    {
        //returnPosition = transform.position;
        flightDestination = returnPosition;
        flightDestination += new Vector3(0, 8, 0);

        state = 1;
       
        child.SetActive(false);
        audioSource.Play();
    }

    public void Return()
    {


        state = 3;
		//Debug.Log("playing coo sound");
		audioSource.Play();
		Debug.Log("" + audioSource.isPlaying);
        
    }




    // Update is called once per frame
    void Update () {

        if (state == 1)
        {

            transform.position = Vector3.Lerp(transform.position, flightDestination, Time.deltaTime * flightSpeed);
        }



        if (state == 3)
        {
            
            transform.position = Vector3.Lerp(transform.position, returnPosition, Time.deltaTime * flightSpeed);
        }

        if (transform.position == flightDestination)
        {
            state = 2;

        }

        if (transform.position == returnPosition && state == 3)
        {
            //Debug.Log("returned.");
            child.SetActive(true);
            state = 4;
        }

        if (state == 4)
        {
            state = 0;
        }

  
	}
}
