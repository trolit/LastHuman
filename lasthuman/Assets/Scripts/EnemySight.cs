using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

    [SerializeField]
    private Enemy enemy; 

	void OnTriggerEnter2D(Collider2D other)
    {
        // if enemy sees Player, - make him as target
        if(other.tag == "Player")
        {
            enemy.Target = other.gameObject;
        }
    }

    // when player exits the sight of enemy
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            enemy.Target = null;
        }
    }
}
