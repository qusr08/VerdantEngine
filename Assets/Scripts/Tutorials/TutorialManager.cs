using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TutorialType {
    MapTutorial,
    GardenTutorial,
    CombatTutorial,
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject stepSelectorMenu;
    [SerializeField] private List<GameObject> gardenTutorialSteps;

    private bool tutorialIsActive = false;
    private TutorialType currentTutorialType;
    private int currentStep = 0;

    public void StartTutorial(int tutorialType) {

        if (!tutorialIsActive) {
            currentTutorialType = (TutorialType)tutorialType;
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
        }
        
        DeactivateAllTutorialSteps();
        steps[currentStep].SetActive(true);
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
}
