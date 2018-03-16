using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    public AudioSource audioPlayerSource;
    public AudioClip punch1;
    public AudioClip punch2;
    public AudioClip punch3;
    public AudioClip punch4;
    public AudioClip fire;
    public AudioClip ice;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void playfire()
    {
        audioPlayerSource.clip = fire;
        audioPlayerSource.Play();
    }

    public void playice()
    {
        audioPlayerSource.clip = ice;
        audioPlayerSource.Play();
    }
}
