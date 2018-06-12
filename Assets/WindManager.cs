using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WindManager : MonoBehaviour {

    public List<AffectedByWind> windAffectedObjects = new List<AffectedByWind>();


    public float windSpeed = 4;

    public float fadeInAmount = 0.01f;
    public float fadeInTick = 0.01f;

    public float playTimeMin = 0.5f;
    public float playTimeMax = 8.0f;

    public float pauseTimeMin = 0.5f;
    public float pauseTimeMax = 32.0f;

    public float fadeOutAmount = 0.005f;
    public float fadeOutTick = 0.01f;

    float random;

   public  List<GameObject> unsortedObjects = new List<GameObject>();
   public List<GameObject> sortedObjects = new List<GameObject>();



    // Use this for initialization
    void Start () {

        

        unsortedObjects = ArrayToList(GameObject.FindGameObjectsWithTag("AffectedByWind"));

        sortedObjects = SortByDistanceFrom(unsortedObjects, gameObject.transform.position);

        for(int i=0; i < sortedObjects.Count; i++)
        {
      

            if (!sortedObjects[i].GetComponent<AffectedByWind>())
            {
                Debug.Log("" + sortedObjects[i].name + " has no AffectedByWind script!");
                return;
            }

            sortedObjects[i].GetComponent<AffectedByWind>().distanceFromWindSource = Vector3.Distance(gameObject.transform.position, sortedObjects[i].transform.position);
            windAffectedObjects.Add(sortedObjects[i].GetComponent<AffectedByWind>());
        }

        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        StartCoroutine(WindStart());
        //Debug.Log("Wind on...");
        yield return new WaitForSeconds(Random.Range(playTimeMin, playTimeMax));

        StartCoroutine(WindStop());
        //Debug.Log("Wind off...");
        yield return new WaitForSeconds(Random.Range(pauseTimeMin, pauseTimeMax));

        StartCoroutine(MainLoop());
    }

    IEnumerator WindStart()
    {
        for (int i = 0; i < windAffectedObjects.Count; i++)
        {
            StopCoroutine(windAffectedObjects[i].FadeOut(this.fadeOutAmount, this.fadeOutTick));
            StartCoroutine(windAffectedObjects[i].FadeIn(this.fadeInAmount, this.fadeInTick));
            yield return new WaitForSeconds(windAffectedObjects[i].distanceFromWindSource / windSpeed);
        }
        yield return null;
    }

    IEnumerator WindStop()
    {
        for (int i = 0; i < windAffectedObjects.Count; i++)
        {
            StopCoroutine(windAffectedObjects[i].FadeIn(this.fadeInAmount, this.fadeInTick));
            StartCoroutine(windAffectedObjects[i].FadeOut(this.fadeOutAmount, this.fadeOutTick));
            yield return new WaitForSeconds(windAffectedObjects[i].distanceFromWindSource / windSpeed);
        }
        yield return null;
    }

    List<GameObject> SortByDistanceFrom(List<GameObject> unsortedList, Vector3 referencePoint)
    {
        List<GameObject> sortedList = new List<GameObject>();

        while (unsortedList.Count > 0)
        {

            int closestObjectIndex = 0;
            float lowestDistance = Vector3.Distance(referencePoint, unsortedList[0].transform.position);

            for (int i = 0; i < unsortedList.Count; i++)
            {
                float distance = Vector3.Distance(referencePoint, unsortedList[i].transform.position);

                if (distance <= lowestDistance)
                {
                    lowestDistance = distance;
                    closestObjectIndex = i;
                }
            }

            sortedList.Add(unsortedList[closestObjectIndex]);
            unsortedList.RemoveAt(closestObjectIndex);
        }

        return sortedList;
    }

    List<GameObject> ArrayToList(GameObject[] arr)
    {
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < arr.Length; i++)
        {
            list.Add(arr[i]);
        }

        return list;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("starting cooroutine...");
            StartCoroutine(WindStart());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("starting cooroutine...");
            StartCoroutine(WindStop());
        }

    }
}
