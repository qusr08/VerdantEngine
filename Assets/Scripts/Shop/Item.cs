using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.ShowToolTip_Static(transform.GetChild(transform.childCount - 1).GetComponent<Plant>().Description.ToString());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.HideToolTip_Static();
    }
}
