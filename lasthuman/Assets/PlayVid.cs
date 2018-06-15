using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MovieTexture movie = GetComponent<Renderer>().material.mainTexture as MovieTexture;
        movie.Play();
        movie.loop = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

}