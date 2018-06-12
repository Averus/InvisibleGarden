using UnityEngine;
using System.Collections;

public class IntermittentSound : MonoBehaviour
{

    AudioSource audioSource;

    enum State { fadingIn, playing, fadingOut, paused };
    State currentState;

    public float maxVolume = 1;

    public float fadeInAmount = 0.2f;
    public float fadeInTick = 0.01f;

    public float playTimeMin = 0.5f;
    public float playTimeMax = 8.0f;

    public float pauseTimeMin = 0.5f;
    public float pauseTimeMax = 5.0f;

    public float fadeOutAmount = 0.075f;
    public float fadeOutTick = 0.01f;

    float random;


    // Use this for initialization
    void Awake()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.Play();
        audioSource.loop = true;

        SkipToRandom();
        StartCoroutine(FadeIn());

    }

    IEnumerator PlayFor(float seconds)
    {
        //Debug.Log("Playing...");
        yield return new WaitForSeconds(seconds);
       //Debug.Log("...Pausing");
        StartCoroutine(FadeOut());
    }

    void SkipToRandom() //Skips the audio to a random point. For ensuring that object swith the same ambient audio dont sound like echos of eachother.
    {
        float r = Random.Range(0f, 1f);
        audioSource.GetComponent<AudioSource>().time = r * GetComponent<AudioSource>().clip.length;
    }

    IEnumerator FadeIn()
    {
        audioSource.Play();
        while (audioSource.volume < maxVolume)
        {
            float f = audioSource.volume;
            f += fadeInAmount;
            audioSource.volume = f;
            yield return new WaitForSeconds(fadeInTick);
            //Debug.Log(f);
        }
        StartCoroutine(PlayFor(Random.Range(playTimeMin, playTimeMax)));

    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0)
        {
            float f = audioSource.volume;
            f -= fadeOutAmount;
            audioSource.volume = f;
            yield return new WaitForSeconds(fadeOutTick);
            //Debug.Log(f);
        }
        StartCoroutine(PauseFor(Random.Range(pauseTimeMin, pauseTimeMax)));

    }

    IEnumerator PauseFor(float seconds)
    {
        audioSource.Pause();
       // Debug.Log("Paused");
        yield return new WaitForSeconds(seconds);

      //  Debug.Log("...Resuming");
        StartCoroutine(FadeIn());

    }




    // Update is called once per frame
    void Update()
    {






        //random = Random.Range(0, 300);
    }
}