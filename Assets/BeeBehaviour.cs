using UnityEngine;
using System.Collections;

public class BeeBehaviour : MonoBehaviour {

    AudioSource audioSource;

    enum State {fadingIn, playing, fadingOut, paused};
    State currentState;

    public float maxVolume = 1;

    public float fadeInAmount = 0.2f;
    public float fadeInTick = 0.01f;

    float playTimeMin = 0.5f;
    float playTimeMax = 8.0f;

    float pauseTimeMin = 0.5f;
    float pauseTimeMax = 5.0f;

    public float fadeOutAmount = 0.075f;
    public float fadeOutTick = 0.01f;

    float random;


    // Use this for initialization
    void Start()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.Play();
        audioSource.loop = true;
       
        StartCoroutine(FadeIn());

    }

    IEnumerator PlayFor(float seconds)
    {

        yield return new WaitForSeconds(seconds);

        StartCoroutine(FadeOut());
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
    
        }
        StartCoroutine(PlayFor(Random.Range(playTimeMin,playTimeMax)));
      
    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0)
        {
            float f = audioSource.volume;
            f -= fadeOutAmount;
            audioSource.volume = f;
            yield return new WaitForSeconds(fadeOutTick);
     
        }
        StartCoroutine(PauseFor(Random.Range(pauseTimeMin, pauseTimeMax)));

    }

    IEnumerator PauseFor(float seconds)
    {
        audioSource.Pause();

        yield return new WaitForSeconds(seconds);

        StartCoroutine(FadeIn());

    }




    // Update is called once per frame
    void Update()
    {

      




        //random = Random.Range(0, 300);
    }
}