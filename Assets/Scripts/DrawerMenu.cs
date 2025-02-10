using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawerMenu : MonoBehaviour
{
    public RectTransform menuPanel;
    public float slideDuration = 0.3f; // Time in seconds for animation
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isOpen = false;
    private Coroutine animationCoroutine; // To prevent overlapping animations

    void Start()
    {
        // Get the panel width
        float width = menuPanel.rect.width;

        // Set positions (Assuming the menu slides in from the left)
        hiddenPosition = new Vector3( 0, 0);
        visiblePosition = new Vector2(-width, 0);

        // Start with the menu hidden
        menuPanel.anchoredPosition = hiddenPosition;
    }

    public void ToggleMenu()
    {
        // Stop any running animation before starting a new one
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        // Start the animation coroutine
        animationCoroutine = StartCoroutine(AnimateMenu(isOpen ? hiddenPosition : visiblePosition));

        // Toggle the menu state
        isOpen = !isOpen;
    }

    private IEnumerator AnimateMenu(Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = menuPanel.anchoredPosition;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            t = t * t * (3f - 2f * t); // Smoothstep easing
            menuPanel.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

        menuPanel.anchoredPosition = targetPosition;
    }
}
