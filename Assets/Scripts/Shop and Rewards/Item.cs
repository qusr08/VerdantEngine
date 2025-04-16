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
    public Image[] raritySlots;
    public Sprite rarityStar;

    public bool needTooltip;

    public Plant plant;
    public Artifact artifact;
    public PlayerAttackSO part;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (needTooltip)
        {
            ToolTip.ShowToolTip_Static(plant.Description);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (needTooltip)
        {
            ToolTip.HideToolTip_Static();

        }
    }

    public void FillShopItemDetails(string itemName, int cost, Sprite displayImage, string description, int rarity)
    {
        this.itemName.text = itemName;
        this.cost.text = cost.ToString();
        this.displayImage.sprite = displayImage;
        this.description.text = description;

        for(int i = 0; i < rarity; i++)
        {
            raritySlots[i].sprite = rarityStar;
        }
    }

    public void FillRewardItemDetails(string itemName, Sprite displayImage)
    {
        this.itemName.text = itemName;
        this.displayImage.sprite = displayImage;
    }

}
