using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> hostUIComponents = new List<GameObject>();
    [SerializeField] private List<GameObject> joinUIComponents = new List<GameObject>();
    [SerializeField] private List<GameObject> defaultUIComponents = new List<GameObject>();

    private void Start()
    {
        ShowDefaultUI();
    }

    public void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
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
        DisableAllChildren();
        EnableAllUIComponents(joinUIComponents);
    }

    public void ShowHostUI()
    {
        DisableAllChildren();
        EnableAllUIComponents(hostUIComponents);
    }

    public void ShowDefaultUI()
    {
        DisableAllChildren();
        EnableAllUIComponents(defaultUIComponents);
    }
}
