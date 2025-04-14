using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "EventPreset", menuName = "ScriptableObjects/EventPreset", order = 2)]

public class Event_SO : ScriptableObject
{
    public string title;
    public string text;
    public Image image;
    public EventOutcome_SO [] Options;
    
}
