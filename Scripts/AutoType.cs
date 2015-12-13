using UnityEngine;
using System.Collections;

public class AutoType : MonoBehaviour
{
    public float letterPause = .01f;
    private GameObject gui;
    //public AudioClip sound;

    string message;

    // Use this for initialization
    void Start()
    {
        gui = GameObject.FindGameObjectWithTag("GUI");
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            gui.GetComponent<GuiScript>().typedText += letter;
            //if (sound)
            //    audio.PlayOneShot(sound);
            //yield return 0;
            yield return new WaitForSeconds(.025f);
        }
        gui.GetComponent<GuiScript>().typingDone = true;
    }

    public void doTypeText()
    {
        message = gui.GetComponent<GuiScript>().typedText;
        gui.GetComponent<GuiScript>().typedText = "";
        StartCoroutine(TypeText());
        
    }
}