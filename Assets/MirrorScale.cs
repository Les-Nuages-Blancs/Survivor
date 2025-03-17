using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScale : MonoBehaviour
{
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private bool mirrorX = true;
    [SerializeField] private bool mirrorY = false;
    [SerializeField] private float mirrorDuration = 0.5f;

    private bool isAnimating = false;
    private Vector3 originalScale;

    public void Mirror()
    {
        if (!isAnimating)
        {
            // Start the mirror effect coroutine
            originalScale = targetTransform.localScale;
            isAnimating = true;
            StartCoroutine(MirrorEffect());
        }
    }

    private IEnumerator MirrorEffect()
    {
        Vector3 targetScale = targetTransform.localScale;

        // Modify scale based on mirrorX and mirrorY flags
        if (mirrorX) targetScale.x = -originalScale.x;
        if (mirrorY) targetScale.y = -originalScale.y;

        float elapsedTime = 0f;

        while (elapsedTime < mirrorDuration)
        {
            // Smoothly interpolate between current and target scale
            targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, elapsedTime / mirrorDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale is the target scale
        targetTransform.localScale = targetScale;
        isAnimating = false;
    }
}

