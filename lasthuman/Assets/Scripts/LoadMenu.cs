using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public Text progresstext;
    public float timer = 1.5f;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            StartCoroutine(LoadAsynchronously());
        }
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progresstext.text = progress * 100f + "%";

            yield return null;
        }
    }
}
