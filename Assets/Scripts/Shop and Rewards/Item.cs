using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text itemName;
    public TMP_Text cost;
    public Image displayImage;
    public TMP_Text description;

    public bool needTooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (needTooltip)
        {
            ToolTip.ShowToolTip_Static(transform.GetChild(transform.childCount - 1).GetComponent<Plant>().Description.ToString());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (needTooltip)
        {
            ToolTip.HideToolTip_Static();

        }
    }

    public void FillShopItemDetails(string itemName, int cost, Sprite displayImage, string description)
    {
        this.itemName.text = itemName;
        this.cost.text = cost.ToString();
        this.displayImage.sprite = displayImage;
        this.description.text = description;
    }

    public void FillRewardItemDetails(string itemName, Sprite displayImage)
    {
        this.itemName.text = itemName;
        this.displayImage.sprite = displayImage;
    }

}
