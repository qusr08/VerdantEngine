using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantHover : MonoBehaviour
{
    [SerializeField] private TMP_Text plantName;
    [SerializeField] private TMP_Text plantDescription;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string name, string description)
    {
        plantName.text = name;
        plantDescription.text = description;
    }
}
