using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public Text displayText;

    public void DisplayText()
    {
        displayText.text = "SETTINGS SAVED!";
        Invoke("HideText", 1.5f);
    }

    public void HideText()
    {
        displayText.text = "";
    }
}