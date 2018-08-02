using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillsSaver : MonoBehaviour
{
    public static int warriorsKilled;
    public static int ogresKilled;
    public static int zombiesKilled;

    [SerializeField]
    private Text warriorsKilledText;
    [SerializeField]
    private Text ogresKilledText;
    [SerializeField]
    private Text zombiesKilledText;

	// Use this for initialization
	void Start ()
    {
		warriorsKilled = PlayerPrefs.GetInt("warriorKill");
        ogresKilled = PlayerPrefs.GetInt("ogreKill");
        zombiesKilled = PlayerPrefs.GetInt("zombieKill");

        warriorsKilledText.text = warriorsKilled.ToString();
        ogresKilledText.text = ogresKilled.ToString();
        zombiesKilledText.text = zombiesKilled.ToString();
    }
}
