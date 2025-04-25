using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartHover : MonoBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;


    [SerializeField] private TMP_Text _Damage;
    [SerializeField] private TMP_Text _CoolDown;
    [SerializeField] private TMP_Text _energy;


    public void UpdateText(PlayerAttackMenuItem part)
    {
        NameText.text = part._playerAttack.Name;
        DescriptionText.text = part._playerAttack.Description;

        _Damage.text = part._playerAttack.Damage.ToString();
        _CoolDown.text = part._playerAttack.MaxCooldown.ToString();
        _energy.text = part._playerAttack.ManaCost.ToString();

    }
}
