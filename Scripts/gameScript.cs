using UnityEngine;
using System.Collections;

public class gameScript : MonoBehaviour {
    public GameObject musicManager;
    public GameObject player;

    public bool repeat1 = false;
    public bool repeat2 = false;
    public bool repeat3 = false;

    public musicManagerScript mMscript;

    // Use this for initialization
    void Start()
    {
        mMscript = musicManager.GetComponent<musicManagerScript>();
        mMscript.loadSource(mMscript.source1, mMscript.track1);
        mMscript.playSource(mMscript.source1);
        mMscript.swap = true;
        //mMscript.source1.GetComponent<audioSourceScript>().audioVolume = 0;

        mMscript.loadSource(mMscript.source2, mMscript.track2);
        mMscript.playSource(mMscript.source2);

        //mMscript.source1.GetComponent<audioSourceScript>().audioVolume = 1;
        mMscript.fadeSpeed = 1f;
        //mMscript.source1.GetComponent<audioSourceScript> ().targetPitch = 2f;

    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
