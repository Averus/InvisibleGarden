using UnityEngine;
using System.Collections;

public class AudioZone : MonoBehaviour
{

    //This class isnt as reusable as it could be - it really needs to define a target and then be divided into methods to calculate disnatce from that target and then alter the audio accordingly.

    AudioSource audioSource;
    GameObject player;

    public float zoneRadius = 0f; // This should be equal to the minimum distance setting in the AudioSource. Thus the volume will be at maximum while you are inside the width of the object.
                                  //Inside the zoneRadius the AudioSource's pan will be set to 0, creating omnidirectional sound.

    public float audioDropoffDistance; // The AudioSource's pan will fall logarithmicly between this value and the ZoneRadius

    public float distanceFromZoneCenter;
    public float distanceToBoarder;
    public float percentageDistanceToBoarder;
    public float panLevel;

    public float distanceFromZoneCenter2;


    // Use this for initialization
    void Start()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Main");

        audioDropoffDistance = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {

        distanceFromZoneCenter = Vector3.Distance(gameObject.transform.position, player.transform.position);


        if (distanceFromZoneCenter < 0) // This flips a negative number to its positive equivalent.
        {
            distanceFromZoneCenter = distanceFromZoneCenter - (distanceFromZoneCenter * 2);
        }
        

        if (distanceFromZoneCenter > zoneRadius) //Instead of calculating our distance from the center of the area, we now calculate our distance from the boarder
        {
            distanceToBoarder = distanceFromZoneCenter - zoneRadius;
        }


        percentageDistanceToBoarder = distanceToBoarder / (audioDropoffDistance / 100);


        percentageDistanceToBoarder = 100 - percentageDistanceToBoarder;


        if (distanceToBoarder <= audioDropoffDistance)
        {


            panLevel = 1 - ((percentageDistanceToBoarder * (percentageDistanceToBoarder / 100)) / 100);

            audioSource.spatialBlend = panLevel;


        }



    }
}
