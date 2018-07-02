using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFade : MonoBehaviour {

    public RawImage splashimage;
    public Text text1;
    public Text text2;
    public string loadlevel;

    IEnumerator Start()
    {
        splashimage.canvasRenderer.SetAlpha(0.0f);
        text1.canvasRenderer.SetAlpha(0.0f);
        text2.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(3.0f);
        FadeOut();
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(loadlevel);
    }

    void FadeIn()
    {
        splashimage.CrossFadeAlpha(1.0f,1.5f,false);
        text1.CrossFadeAlpha(1.0f, 1.5f, false);
        text2.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        splashimage.CrossFadeAlpha(0.0f, 2.5f, false);
        text1.CrossFadeAlpha(0.0f, 2.5f, false);
        text2.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}
