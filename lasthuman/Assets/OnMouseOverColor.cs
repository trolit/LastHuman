using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseOverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text t;

    public void OnPointerEnter(PointerEventData eventData)
    {
        t.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        t.color = Color.white;
    }
}