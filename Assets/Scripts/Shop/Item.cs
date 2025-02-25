using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.ShowToolTip_Static("Cost : " + transform.GetChild(1).GetComponent<Plant>().Cost.ToString());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.HideToolTip_Static();
    }
}
