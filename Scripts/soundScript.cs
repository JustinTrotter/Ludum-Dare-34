using UnityEngine;
using System.Collections;

public class soundScript : MonoBehaviour {
    public AudioSource source1;
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip clip)
    {
        source1.GetComponent<AudioSource>().clip = clip;
        source1.GetComponent<AudioSource>().Play();
    }
}
