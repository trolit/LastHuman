using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour {

    [SerializeField]
    private string targetTag;

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == targetTag)
        {
            // if we hit something, simply disable collider
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
