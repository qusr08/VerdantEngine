using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TreeOfLife_StatPopUp : MonoBehaviour
{    [SerializeField] private InfoPopUp _PopUpDisplay;
    [SerializeField] private PlayerDataManager data;
    private void OnMouseEnter()
    {
        
        //Elad did a good job on this :) - Sofia

            _PopUpDisplay.gameObject.SetActive(true);
            _PopUpDisplay.SetUpTree( data);
        

    }

    private void OnMouseExit()
    {

        _PopUpDisplay.gameObject.SetActive(false);

    }
}
