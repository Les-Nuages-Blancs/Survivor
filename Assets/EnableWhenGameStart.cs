using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenGameStart : MonoBehaviour
{
    [SerializeField] private List<Behaviour> components = new List<Behaviour>();

    private void Start()
    {
        LevelStateManager.Instance.onSpawnEntityChanged.AddListener(UpdateVisibility);
        UpdateVisibility();
    }

    private void OnDestroy()
    {
        LevelStateManager.Instance.onSpawnEntityChanged.RemoveListener(UpdateVisibility);
    }

    private void UpdateVisibility()
    {
        if (LevelStateManager.Instance.SpawnEntity)
        {
            EnableAllComponents();
        }
        else
        {
            DisableAllComponents();
        }
    }

    private void DisableAllComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }

    private void EnableAllComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }
}
