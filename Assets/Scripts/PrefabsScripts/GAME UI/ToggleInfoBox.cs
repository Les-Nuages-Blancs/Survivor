using UnityEngine;
using System.Collections;

public class ToggleInfoBox : MonoBehaviour
{
    public RectTransform infoBox; 
    public float slideSpeed = 0.5f; 
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;

    void Start()
    {
        visiblePosition = infoBox.anchoredPosition;
        hiddenPosition = new Vector2(infoBox.rect.width, infoBox.anchoredPosition.y); 
        infoBox.anchoredPosition = hiddenPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StopAllCoroutines();
            StartCoroutine(TogglePanel());
        }
    }

    IEnumerator TogglePanel()
    {
        float elapsedTime = 0f;
        Vector2 startPos = isVisible ? visiblePosition : hiddenPosition;
        Vector2 targetPos = isVisible ? hiddenPosition : visiblePosition;

        while (elapsedTime < slideSpeed)
        {
            elapsedTime += Time.deltaTime;
            infoBox.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / slideSpeed);
            yield return null;
        }

        infoBox.anchoredPosition = targetPos;
        isVisible = !isVisible;
    }
}
