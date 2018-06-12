using UnityEngine;
using System.Collections;

public class StreamWidth : MonoBehaviour {

    //This class isnt as reusable as it could be - it really needs to define a target and then be divided into methods to calculate disnatce from that target and then alter the audio accordingly.

    AudioSource audioSource;
    GameObject player;

    public float widthInMeters = 5f; // This should be equal to 2* the minimum distance setting in the AudioSource. Thus the volume will be at maximum while you are inside the width of the object.

    public float distanceFromStream;
    public float percentageDistanceFromStream;
    public float invertedPercentage;

    public float distanceToBoarder;
    public float percentageDistanceToBoarder;
    public float maxAudioDropoffDistance;
    public float minAudioDropoffDistance;

    public float panlevel;

	// Use this for initialization
	void Start () {

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Main");

        maxAudioDropoffDistance = 10;
    }
	
	// Update is called once per frame
	void Update () {


        distanceFromStream = gameObject.transform.position.x;
        distanceFromStream = distanceFromStream - player.transform.position.x;

        if(distanceFromStream < 0) // This flips a negative number to its positive equivalent.
        {
            distanceFromStream = distanceFromStream - (distanceFromStream * 2);
        }


        if( distanceFromStream > (widthInMeters / 2)) //Instead of calculating our distance from the center of the area, we now calculate our distance from the boarder on the x axis
        {
            //distanceFromStream = distanceFromStream - (widthInMeters / 2);
            distanceToBoarder = distanceFromStream - (widthInMeters/2);
        }



        //Experimenting past this point...

        percentageDistanceToBoarder = distanceToBoarder / (maxAudioDropoffDistance / 100);


        invertedPercentage = 100 - percentageDistanceToBoarder;

        minAudioDropoffDistance = 000;
        if (distanceToBoarder <= maxAudioDropoffDistance)
        {


            panlevel = 1 - ((invertedPercentage * (invertedPercentage / 100)) / 100);

            audioSource.spatialBlend = panlevel;

        
        }




        /*
        //THE CODE BELOW IS WORKING AND 'KNOWN GOOD' REVERT TO IT IF EVRYTHING BREAKS LOL

                percentageDistanceFromStream = distanceFromStream / ((widthInMeters / 2) / 100);


                invertedPercentage = 100 - percentageDistanceFromStream;

                //panlevel = Mathf.Log(2, percentageDistanceFromStream);
                //panlevel = ((percentageDistanceFromStream * (percentageDistanceFromStream/100))/100);


               if (distanceFromStream <= (widthInMeters / 2))
               {

                    panlevel = 1 - ((invertedPercentage * (invertedPercentage / 100)) / 100);

                    audioSource.panLevel = panlevel;


                }

        */


    }
}
