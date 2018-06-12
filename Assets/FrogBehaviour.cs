using UnityEngine;
using System.Collections;

public class FrogBehaviour : MonoBehaviour {


    int random;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {

        audioSource = gameObject.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        random = Random.Range(0, 300);

        if (random == 1 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }



        if (Input.GetKeyDown(KeyCode.L))
        {
                audioSource.Play();
            }
	
	}
}
