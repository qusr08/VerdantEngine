using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "EventPreset", menuName = "ScriptableObjects/EventPreset", order = 2)]

public class Event_SO : ScriptableObject
{
    public string title;
    public string text;
    public Sprite image;
    public EventOutcome_SO [] Options;

    public void ForceCombat(CombatPresetSO combat)
    {
        foreach (EventOutcome_SO outcomes in Options)
        {
            outcomes.ForceCombat = true;
            outcomes.combatPresetSO = combat;
        }
    }
    
}
