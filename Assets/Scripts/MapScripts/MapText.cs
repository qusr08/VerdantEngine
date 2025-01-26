using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtEvent(string type, string rewards = "")
    {

        if(rewards != "")
        {
            text.text = "" + type + " - " + rewards;
            return;
        }

        text.text = "" + type;
    }
}
