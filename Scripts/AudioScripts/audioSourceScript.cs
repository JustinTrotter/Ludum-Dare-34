using UnityEngine;
using System.Collections;

public class audioSourceScript : MonoBehaviour
{

    public GameObject audioManager;
    public AudioClip track;
    public float maxVolume = 1f;
    public float audioVolume = 0.0f;
    public float audioPitch = 1.0f;
    public float targetPitch = 1.0f;
    public float pitchRate = 0.1f;

    // Use this for initialization
    void Start()
    {
        GetComponent<AudioSource>().volume = audioVolume;
    }

    // Update is called once per frame
    void Update()
    {
        updatePitch();
    }

    void updatePitch()
    {

        if (targetPitch > audioPitch)
            pitchUp();
        else if (targetPitch < audioPitch)
            pitchDown();

        //Stabilize Pitch
        if (Mathf.Abs(audioPitch - targetPitch) < pitchRate * 0.1f)
        { // * .1 is offset for Time.deltaTime
            audioPitch = targetPitch;
            GetComponent<AudioSource>().pitch = audioPitch;
        }
    }

    public void fadeIn(float rate)
    {
        if (audioVolume < maxVolume)
        {
            audioVolume += rate * Time.deltaTime;
            GetComponent<AudioSource>().volume = audioVolume;
        }
    }

    public void fadeOut(float rate)
    {
        if (audioVolume > 0.001f)
        {
            audioVolume -= rate * Time.deltaTime;
            GetComponent<AudioSource>().volume = audioVolume;
        }
    }

    public void pitchUp()
    {
        if (audioPitch < targetPitch)
        {
            audioPitch += pitchRate * Time.deltaTime;
            GetComponent<AudioSource>().pitch = audioPitch;
        }
    }

    public void pitchDown()
    {
        if (audioPitch > targetPitch)
        {
            audioPitch -= pitchRate * Time.deltaTime;
            GetComponent<AudioSource>().pitch = audioPitch;
        }
    }
}