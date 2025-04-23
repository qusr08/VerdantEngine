using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventOutcomeType
{
    NewFlower,
    NewArtifact,
    GetMoeny,
    Health,
    Combat,
    Part
}
[CreateAssetMenu(fileName = "EventOutcome_SO", menuName = "ScriptableObjects/EventOutcome_SO", order = 2)]

public class EventOutcome_SO : ScriptableObject
{
    public string outcomeText;
    public EventOutcomeType outcomeType;
    public PlantType[] potinalPlantReward;
    public ArtifactType[] potinalArtifactReward;
    public PlayerAttackSO [] potinalPartReward;
    
    public int MoneyChange;
    public int HealthChange;
    public bool ForceMoenyChange;
    public bool ForceHealthChange;
    public bool ForceCombat;
    public Sprite resultWindowImage;
    public string resultWindowText;

    public CombatPresetSO combatPresetSO;

}
