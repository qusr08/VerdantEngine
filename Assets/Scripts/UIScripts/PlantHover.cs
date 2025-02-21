using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantHover : MonoBehaviour
{
    [SerializeField] private TMP_Text plantName;
    [SerializeField] private TMP_Text plantDescription;
    [SerializeField] private TMP_Text plantHP;
    [SerializeField] private Image plantImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string name, string description, Stat HealthStat, Sprite image)
    {
        plantName.text = name;
        plantDescription.text = description;
        plantHP.text = "" + HealthStat.CurrentValue + "/" + HealthStat.MaxValue;
        plantImage.sprite = image;
    }
}
