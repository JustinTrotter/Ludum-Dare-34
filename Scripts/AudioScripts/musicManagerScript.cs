using UnityEngine;
using System.Collections;

public class musicManagerScript : MonoBehaviour
{

    public AudioClip track1;
    public AudioClip track2;
    public AudioClip track3;

    public AudioSource source1;
    public AudioSource source2;

    public bool swap = false;
    public float fadeSpeed = 0.25f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateVolume();
    }

    void updateVolume()
    {
        if (swap)
        {
            source2.GetComponent<audioSourceScript>().fadeIn(fadeSpeed);
            source1.GetComponent<audioSourceScript>().fadeOut(fadeSpeed);
        }
        else {
            source1.GetComponent<audioSourceScript>().fadeIn(fadeSpeed);
            source2.GetComponent<audioSourceScript>().fadeOut(fadeSpeed);
        }
    }

    void OnGUI()
    {
        		//GUI.Label(new Rect(10, 10, 200, 100), "Audio 1 : " + source1.GetComponent<crossFade>().audioVolume.ToString());
        		//GUI.Label(new Rect(10, 30, 200, 100), "Audio 2 : " + source2.GetComponent<crossFade>().audioVolume.ToString());
    }

    public void toggleMusic()
    {
        if (swap)
            swap = false;
        else
            swap = true;
    }

    public void changeTrack(AudioClip clip)
    {
        if (swap)
        {
            loadSource(source1, clip);
            playSource(source1);
        }
        else {
            loadSource(source2, clip);
            playSource(source2);
        }
        toggleMusic();
    }

    public void loadSource(AudioSource source, AudioClip clip)
    {
        source.GetComponent<AudioSource>().clip = clip;
    }

    public void playSource(AudioSource source)
    {
        source.GetComponent<AudioSource>().Play();
    }
}