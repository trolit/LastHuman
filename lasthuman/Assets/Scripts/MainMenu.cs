using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
        
    }

    public void QuitGame()
    {
        // Debug.Log("quit");
        Application.Quit();
    }
}
