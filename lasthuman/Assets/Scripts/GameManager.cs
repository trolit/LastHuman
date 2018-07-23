using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    // singleton to reach it everywhere
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();

            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    [SerializeField]
    private Text soulTxt;

    private int collectedSouls;

    public int CollectedSouls
    {
        get
        {
            return collectedSouls;
        }

        set
        {
            soulTxt.text = value.ToString();
            this.collectedSouls = value;
        }
    }

    [SerializeField]
    private GameObject soulPrefab;

    public GameObject SoulPrefab
    {
        get
        {
            return soulPrefab;
        }
    }

    [SerializeField]
    private ParticleSystem bloodEffect;

    public ParticleSystem BloodEffect
    {
        get
        {
            return bloodEffect;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
