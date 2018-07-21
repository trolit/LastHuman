using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    // sound effects(SFX)
    public static AudioClip hurtplayer01, hurtplayer02, hurtplayer03, slashhit01, slashhit02, slashhit03, slashhit04,
        slashmiss01, slashmiss02, slashmiss03;
    static AudioSource audioSrc;

    // Use this for initialization
    void Start () {
        hurtplayer01 = Resources.Load<AudioClip>("Hurtplayer_01");
        hurtplayer02 = Resources.Load<AudioClip>("Hurtplayer_02");
        hurtplayer03 = Resources.Load<AudioClip>("Hurtplayer_03");

        slashhit01 = Resources.Load<AudioClip>("Slash_hit01");
        slashhit02 = Resources.Load<AudioClip>("Slash_hit02");
        slashhit03 = Resources.Load<AudioClip>("Slash_hit03");
        slashhit04 = Resources.Load<AudioClip>("Slash_hit04");

        slashmiss01 = Resources.Load<AudioClip>("Slash_miss01");
        slashmiss02 = Resources.Load<AudioClip>("Slash_miss02");
        slashmiss03 = Resources.Load<AudioClip>("Slash_miss03");

        audioSrc = GetComponent<AudioSource>();
    }
	
	public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "Hurtplayer_01":
                audioSrc.PlayOneShot(hurtplayer01);
                break;
            case "Hurtplayer_02":
                audioSrc.PlayOneShot(hurtplayer02);
                break;
            case "Hurtplayer_03":
                audioSrc.PlayOneShot(hurtplayer03);
                break;
            case "Slash_hit01":
                audioSrc.PlayOneShot(slashhit01);
                break;
            case "Slash_hit02":
                audioSrc.PlayOneShot(slashhit02);
                break;
            case "Slash_hit03":
                audioSrc.PlayOneShot(slashhit03);
                break;
            case "Slash_hit04":
                audioSrc.PlayOneShot(slashhit04);
                break;
            case "Slash_miss01":
                audioSrc.PlayOneShot(slashmiss01);
                break;
            case "Slash_miss02":
                audioSrc.PlayOneShot(slashmiss02);
                break;
            case "Slash_miss03":
                audioSrc.PlayOneShot(slashmiss03);
                break;
        }
    }
}
