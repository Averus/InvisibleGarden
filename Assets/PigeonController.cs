using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PigeonController : MonoBehaviour {


    public float scareRadius = 5;
    public float playerDistanceFromScare;
    public bool pigeonsScared = false;
	public float scareTime = 90;
    GameObject player;


    public PigeonBehaviour[] pigeons = new PigeonBehaviour[5];

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Main");

       pigeons[0] = GameObject.Find("PigeonFly1").GetComponent<PigeonBehaviour>();
       pigeons[1] = GameObject.Find("PigeonFly2").GetComponent<PigeonBehaviour>();
       pigeons[2] = GameObject.Find("PigeonFly3").GetComponent<PigeonBehaviour>();
       pigeons[3] = GameObject.Find("PigeonFly4").GetComponent<PigeonBehaviour>();
       pigeons[4] = GameObject.Find("PigeonFly5").GetComponent<PigeonBehaviour>();



    }

    IEnumerator ScarePigeons()
    {
        for(int i = 0; i < pigeons.Length; i++)
        {
            //Debug.Log("for pigeon " + i);

            yield return new WaitForSeconds(Random.Range(0.1f, 1f));

            pigeons[i].FlyAway();

            //Debug.Log("pigeon " + i + " flying away");
        }
    }

    IEnumerator PigeonsReturn()
    {
        for (int i = 0; i < pigeons.Length; i++)
        {
            //Debug.Log("for pigeon " + i);

            yield return new WaitForSeconds(Random.Range(0.1f, 1f));

            pigeons[i].Return();

            //Debug.Log("pigeon " + i + " returning");
        }
    }

	IEnumerator PigeonWait()
	{
		yield return new WaitForSeconds(scareTime);
		//Debug.Log("Return!");
		pigeonsScared = false;
		StartCoroutine(PigeonsReturn());
	}


    // Update is called once per frame
    void Update () {

        playerDistanceFromScare = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (playerDistanceFromScare < scareRadius && !pigeonsScared)
        {
            //Debug.Log("Boo!");
            StartCoroutine(ScarePigeons());
			StartCoroutine(PigeonWait());
            pigeonsScared = true;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Return!");
            pigeonsScared = false;
            StartCoroutine(PigeonsReturn());
        }


    }
}
