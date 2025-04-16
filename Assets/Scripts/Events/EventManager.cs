using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject eventPanal;
    [SerializeField] TextMeshProUGUI eventTitle;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] Image eventImage;
    [SerializeField] GameObject[] buttons;
    [SerializeField] TextMeshProUGUI[] buttonsText;

    Event_SO event_SO;
    [SerializeField] PlayerDataManager playerDataManager;
    public CombatManager combatManager;
    [SerializeField] Inventory inventory;
    [SerializeReference] MapPlayer camManager;
    public void InitilazeEvent(Event_SO event_SO)
    {
        eventPanal.SetActive(true);

        this.event_SO = event_SO;
        eventTitle.text = event_SO.title;
        eventImage.sprite = event_SO.image;
        eventText.text = event_SO.text;


        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < event_SO.Options.Length)
            {
                buttons[i].SetActive(true);
                buttonsText[i].text = event_SO.Options[i].outcomeText;
            }
            else
            {
                buttons[i].SetActive(false);

            }
        }

    }

    public void EventOutcomeChosen(int index)
    {
        EventOutcome_SO chosen = event_SO.Options[index];
        switch (chosen.outcomeType)
        {
            case EventOutcomeType.NewFlower:
                PlantType plant;
                if (chosen.potinalPlantReward.Length > 1)
                {
                    plant = chosen.potinalPlantReward[Random.Range(0, chosen.potinalPlantReward.Length)];


                }
                else plant = chosen.potinalPlantReward[0];
                inventory.AddPlant(plant);

                break;
            case EventOutcomeType.NewArtifact:
                ArtifactType artifact;
                if (chosen.potinalPlantReward.Length > 1)
                {
                    artifact = chosen.potinalArtifactReward[Random.Range(0, chosen.potinalArtifactReward.Length)];

                }
                else artifact = chosen.potinalArtifactReward[0];
                inventory.AddArtifact(artifact);
                break;
            case EventOutcomeType.GetMoeny:
                playerDataManager.Money += chosen.MoneyChange;
                break;
            case EventOutcomeType.Health:
                playerDataManager.CurrentHealth += chosen.HealthChange;

                break;
            case EventOutcomeType.Combat:
                combatManager.NewCombat(chosen.combatPresetSO);
                camManager.GoToGarden();
                break;
            default:
                break;
        }
        if(chosen.ForceHealthChange) playerDataManager.CurrentHealth += chosen.HealthChange;
        if (chosen.ForceMoenyChange) playerDataManager.Money += chosen.MoneyChange;

        eventPanal.SetActive(false);
    }

}
