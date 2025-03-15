using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> hostUIComponents = new List<GameObject>();
    [SerializeField] private List<GameObject> joinUIComponents = new List<GameObject>();
    [SerializeField] private List<GameObject> defaultUIComponents = new List<GameObject>();
    [SerializeField] private List<GameObject> allUIComponents = new List<GameObject>();

    private void Start()
    {
        ShowDefaultUI();
    }

    public void DisableAllUI()
    {
        foreach (GameObject uiComponent in allUIComponents)
        {
            uiComponent.SetActive(false);
        }
    }

    public void EnableAllUIComponents(List<GameObject> UIComponents)
    {
        foreach (GameObject UIComponent in UIComponents)
        {
            UIComponent.SetActive(true);
        }
    }

    public void ShowJoinUI()
    {
        DisableAllUI();
        EnableAllUIComponents(joinUIComponents);
    }

    public void ShowHostUI()
    {
        DisableAllUI();
        EnableAllUIComponents(hostUIComponents);
    }

    public void ShowDefaultUI()
    {
        DisableAllUI();
        EnableAllUIComponents(defaultUIComponents);
    }
}
