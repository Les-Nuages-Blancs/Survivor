using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopyTextToClipboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    // Call this function to copy the text to the clipboard
    public void CopyToClipboard()
    {
        if (textMeshPro != null)
        {
            GUIUtility.systemCopyBuffer = textMeshPro.text; // Copy the text to the clipboard
            Debug.Log($"Copied to clipboard: {textMeshPro.text}");
        }
        else
        {
            Debug.LogWarning("TextMeshPro reference is not assigned.");
        }
    }
}
