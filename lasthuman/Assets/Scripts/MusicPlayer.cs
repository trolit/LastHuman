using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public AudioClip[] clips;
    private AudioSource audiosource;

	// Use this for initialization
	void Start () {
        audiosource = FindObjectOfType<AudioSource>();
        audiosource.loop = false;
        audiosource.clip = GetRandomClip();
        audiosource.Play();
    }
	
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

	// Update is called once per frame
	void Update () {

	}
}
