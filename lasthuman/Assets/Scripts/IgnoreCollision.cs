using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private Collider2D other;

	// Use this for initialization
	private void Awake ()
    {
        // Ignore collision with other object which has Collider
        // the other object will be our Player of course
        // and put this script on Enemy

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);

	}
	
}
