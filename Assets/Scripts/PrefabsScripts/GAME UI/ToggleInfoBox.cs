using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ToggleInfoBox : MonoBehaviour
{
    [SerializeField] private RectTransform targetObject; // The object that will move
    [SerializeField] private RectTransform widthReference; // Defines how far it moves
    [SerializeField] private float slideSpeed = 0.5f;
    [SerializeField] private bool startClosed = true;
    [SerializeField] private UnityEvent<bool> onVisibilityChanged;

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;
    private bool isAnimating = false; // Prevent re-triggering animation while running

    void Start()
    {
        if (targetObject == null || widthReference == null)
        {
            Debug.LogError("Target object or width reference is not assigned!");
            return;
        }

        StartCoroutine(InitializeAfterLayout());
    }

    IEnumerator InitializeAfterLayout()
    {
        yield return new WaitForEndOfFrame(); // Wait for UI layout calculations

        visiblePosition = targetObject.anchoredPosition;

        hiddenPosition = new Vector2(visiblePosition.x + widthReference.rect.width, visiblePosition.y);

        targetObject.anchoredPosition = startClosed ? hiddenPosition : visiblePosition;
        isVisible = !startClosed;
    }

    public void StartTogglePanel()
    {
        if (!isAnimating)
        {
            StartCoroutine(TogglePanel());
        }
    }

    IEnumerator TogglePanel()
    {
        isAnimating = true; // Mark animation as running
        float elapsedTime = 0f;
        Vector2 startPos = isVisible ? visiblePosition : hiddenPosition;
        Vector2 targetPos = isVisible ? hiddenPosition : visiblePosition;

        while (elapsedTime < slideSpeed)
        {
            elapsedTime += Time.deltaTime;
            targetObject.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / slideSpeed);
            yield return null;
        }

        targetObject.anchoredPosition = targetPos;
        isVisible = !isVisible;
        isAnimating = false; // Allow new activation after animation is complete
        onVisibilityChanged.Invoke(isVisible);
    }
}
