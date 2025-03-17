using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> inGameUIs = new List<GameObject>();
    [SerializeField] private List<GameObject> globalMenuUIs = new List<GameObject>();
    [SerializeField] private List<GameObject> allUIs = new List<GameObject>();

    private void Start()
    {
        ShowGlobalMenuUI();
    }

    public void DisableAllUI()
    {
        foreach (GameObject uiComponent in allUIs)
        {
            uiComponent.SetActive(false);
        }
    }

    public void EnableUI(List<GameObject> UIComponents)
    {
        foreach (GameObject UIComponent in UIComponents)
        {
            UIComponent.SetActive(true);
        }
    }

    public void ShowInGameUI()
    {
        DisableAllUI();
        EnableUI(inGameUIs);
    }

    public void ShowGlobalMenuUI()
    {
        DisableAllUI();
        EnableUI(globalMenuUIs);
    }

    public void ToggleGlobalMenuWithInGameUI()
    {
        if (globalMenuUIs[0].activeInHierarchy)
        {
            ShowInGameUI();
        }
        else
        {
            ShowGlobalMenuUI();
        }
    }
}
