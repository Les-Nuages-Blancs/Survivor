using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGame : MonoBehaviour
{
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            EditorApplication.isPlaying = false;
#else
            // Quit the application in a build
            Application.Quit();
#endif
        });
    }
}
