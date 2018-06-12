using UnityEngine;
using System.Collections;

public class AffectedByWind : MonoBehaviour
{

    AudioSource audioSource;
    public float distanceFromWindSource = 0;
    public float maxVolume = 1;
    public float minVolume = 0.01f;

   // public float fadeInAmount = 0.2f;
  //  public float fadeInTick = 0.01f;

    float playTimeMin = 0.5f;
    float playTimeMax = 8.0f;

    float pauseTimeMin = 0.5f;
    float pauseTimeMax = 5.0f;

    //public float fadeOutAmount = 0.075f;
   // public float fadeOutTick = 0.01f;

    //float random;


    // Use this for initialization
    void Awake()
    {
        if (!gameObject.GetComponent<AudioSource>())
        {
            Debug.Log("" + gameObject.name + " has no AudioSource!");
        }


        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0f;
        PlaySkipped();
        audioSource.loop = true;

       // StartCoroutine(FadeIn());

    }

    void PlaySkipped() //Skips the audio to a random point and then plays from there. For ensuring that object swith the same ambient audio dont sound like echos of eachother.
    {
        float r = Random.Range(0f, 1f);
        audioSource.GetComponent<AudioSource>().time =  r * GetComponent<AudioSource>().clip.length;
        audioSource.Play();
    }

    IEnumerator PlayFor(float seconds)
    {
        //Debug.Log("Playing...");
        yield return new WaitForSeconds(seconds);

    }

    public IEnumerator FadeIn(float fadeInAmount, float fadeInTick)
    {
        
        while (audioSource.volume < maxVolume)
        {
            float f = audioSource.volume;
            f += fadeInAmount;
            audioSource.volume = f;
            yield return new WaitForSeconds(fadeInTick);
            
        }
        //StartCoroutine(PlayFor(Random.Range(playTimeMin, playTimeMax)));

    }

    public IEnumerator FadeOut(float fadeOutAmount, float fadeOutTick)
    {

        while (audioSource.volume > minVolume)
        {
            float f = audioSource.volume;
            f -= fadeOutAmount;
            audioSource.volume = f;
            yield return new WaitForSeconds(fadeOutTick);
           
        }
        //StartCoroutine(PauseFor(Random.Range(pauseTimeMin, pauseTimeMax)));

    }

    IEnumerator PauseFor(float seconds)
    {
        audioSource.Pause();
        //Debug.Log("Paused");
        yield return new WaitForSeconds(seconds);

        //Debug.Log("...Resuming");
       // StartCoroutine(FadeIn());

    }




    // Update is called once per frame
    void Update()
    {


    }
}