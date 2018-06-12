using UnityEngine;
using System.Collections;

public class nightinggaleBehaviour : MonoBehaviour {



    bool flying = false;
    bool locationA = true;
    bool playWings = false;
    
    Vector3 posA = new Vector3(34.7f, 0f, 2.8f);
    Vector3 posB = new Vector3(2.33f, 0f, 33.14f);

    AudioSource audioSource;
    public AudioClip wings;



    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
       
       
	
	}


    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Main")
        {
            if (!flying)
            {
                flying = true;
                playWings = true;

                
            }
        }
    }

    void PlayWingsSFX()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(wings, 0.6f);
        playWings = false;
    }


    // Update is called once per frame
    void Update () {

        if (!flying && !audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.loop = true;
        }

        if (playWings)
        {
            PlayWingsSFX();
        }





        if (flying && locationA)
        {
            transform.position = Vector3.MoveTowards(transform.position, posB, 0.25f);
        }


        if (flying && locationA && transform.position == posB)
        {
            flying = false;
            locationA = false;
        }

        if (flying && !locationA)
        {
            transform.position = Vector3.MoveTowards(transform.position, posA, 0.25f);
        }


        if (flying && !locationA && transform.position == posA)
        {
            flying = false;
            locationA = true;
        }


    }
}
