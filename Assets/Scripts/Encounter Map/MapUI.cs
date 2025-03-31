using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private MapPlayer cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Currently not called
    public void AtEvent(string type, string rewards = "")
    {

        if(rewards != "")
        {
            text.text = "" + type + " - " + rewards;
            return;
        }

        text.text = "" + type;
    }

    public void HoverEvent(string type, string rewards = "")
    {

        if (rewards != "")
        {
            text.text = "" + type + ": " + rewards;
            return;
        }

        text.text = "" + type;
    }

    public void ToGarden()
    {
        cameraManager.scene = ActiveScene.Garden;
        cameraManager.UpdateCameraPosition();
    }

    public void ToShop()
    {
        cameraManager.scene = ActiveScene.Shop;
        cameraManager.UpdateCameraPosition();
    }
}
