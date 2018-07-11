using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public Text displayText;

    public void DisplayText()
    {
        displayText.text = "SETTINGS SAVED!";
        // run HideText function after 1.5 seconds
        Invoke("HideText", 1.5f);
    }

    public void HideText()
    {
        displayText.text = "";
    }
}