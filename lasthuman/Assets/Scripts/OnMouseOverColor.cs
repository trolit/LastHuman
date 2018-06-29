using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseOverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text t;

    // event function OnDisable .... how long have I been searching for you...
    // red color staying after switching "menu option" was really annoying ...
    // link: https://docs.unity3d.com/Manual/ExecutionOrder.html
    void OnDisable()
    {
        t.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        t.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t.color = Color.white;
    }

}