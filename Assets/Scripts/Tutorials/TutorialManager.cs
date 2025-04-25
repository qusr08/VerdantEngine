using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum TutorialType {
    MapTutorial,
    GardenTutorial,
    CombatTutorial,
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private MapPlayer mapPlayer;

    [SerializeField] private GameObject stepSelectorMenu;
    [SerializeField] private TextMeshProUGUI stepSelectorMenuText;
    [SerializeField] private TextMeshProUGUI previousStepText;
    [SerializeField] private TextMeshProUGUI nextStepText;
    [SerializeField] private TextMeshProUGUI xStepText;

    [SerializeField] private List<GameObject> gardenTutorialSteps;
    [SerializeField] private List<GameObject> mapTutorialSteps;
    [SerializeField] private List<GameObject> combatTutorialSteps;

    [SerializeField] private CombatUIManager uiManager;
    

    private bool tutorialIsActive = false;
    private TutorialType currentTutorialType;
    private int currentStep = 0;

    public void StartTutorial(TutorialType tutorialType) {

        if (!tutorialIsActive) {
            currentTutorialType = tutorialType;
            tutorialIsActive = true;
            OpenStepSelectorMenu();
            GoToStep(0);
        }

    }

    private void GoToStep(int i) {

        List<GameObject> steps = GetTutorialSteps(currentTutorialType);
        currentStep = i;

        if (currentStep < 0) {
            currentStep = 0;
        } else if (currentStep >= steps.Count) {
            EndTutorial();
            return;
        }

        // Change the prev step text
        if (currentStep == 0) {
            previousStepText.text = "";
        } else {
            previousStepText.text = "<";
        }

        // change the next step text
        if (currentStep == steps.Count - 1) {
            nextStepText.gameObject.SetActive(false);
            xStepText.gameObject.SetActive(true);
        } else {
            nextStepText.gameObject.SetActive(true);
            xStepText.gameObject.SetActive(false);
        }
        
        DeactivateAllTutorialSteps();
        steps[currentStep].SetActive(true);
        ChangeStepSelectorMenuText();
    }

    public void NextStep() {

        GoToStep(currentStep + 1);

    }

    public void PreviousStep() {

        GoToStep(currentStep - 1);

    }

    public  void EndTutorial() {

        DeactivateAllTutorialSteps();
        CloseStepSelectorMenu();
        tutorialIsActive = false;
        currentStep = 0;
    }

    private List<GameObject> GetTutorialSteps(TutorialType tutorialType) {

        if (tutorialType == TutorialType.GardenTutorial) {
            return gardenTutorialSteps;
        }
        else if (tutorialType == TutorialType.MapTutorial) {
            return mapTutorialSteps;
        }
        else if (tutorialType == TutorialType.CombatTutorial) {
            return combatTutorialSteps;
        }

        return null;

    }

    private void DeactivateAllTutorialSteps() {

        List<GameObject> steps = GetTutorialSteps(currentTutorialType);

        foreach (GameObject step in steps) {
            step.SetActive(false);
        }

    }

    private void OpenStepSelectorMenu() {

        stepSelectorMenu.SetActive(true);

    }

    private void CloseStepSelectorMenu() {

        stepSelectorMenu.SetActive(false);

    }

    private void ChangeStepSelectorMenuText() {
        List<GameObject> steps = GetTutorialSteps(currentTutorialType);
        string newText = (currentStep + 1) + " / " + steps.Count;
        stepSelectorMenuText.text = newText;
    }

    public void TutorialButtonPressed() {
        if (mapPlayer.scene == ActiveScene.Map) {
            StartTutorial(TutorialType.MapTutorial);
        }
        else if (uiManager.GameState == GameState.COMBAT) {
            StartTutorial(TutorialType.CombatTutorial);
        } 
        else if (mapPlayer.scene == ActiveScene.Garden) {
            StartTutorial(TutorialType.GardenTutorial);
        }

    }
}
