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

    private void EndTutorial() {

        DeactivateAllTutorialSteps();
        CloseStepSelectorMenu();
        tutorialIsActive = false;

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
