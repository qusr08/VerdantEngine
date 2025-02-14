using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponMenuItem : MonoBehaviour
{
   [HideInInspector] public Part_SO part;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI coolDownText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI manaCostText;
    public TextMeshProUGUI detailsText;

    public void SetUp(Part_SO part)
    {
        part.coolDown = part.maxCoolDown;
        this.part = part;
        nameText.text = part.attackName;
        damageText.text = part.damage.ToString();
        coolDownText.text = part.maxCoolDown.ToString();
        manaCostText.text = part.manaCost.ToString();
        detailsText.text = part.attackText;
    }

    public void UpdateCoolDown()
    {
        coolDownText.text = part.coolDown.ToString();
    }
}
