using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskZoneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskLabel;
    private TaskZone taskZone;

    public void InitTaskZone(TaskZone tz)
    {
        if (taskZone == null)
        {
            taskZone = tz;
            taskZone.onUpgraderChange.AddListener(UpdateUI);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("taskZoneUI has already been init");
        }
    }

    private void OnDestroy()
    {
        if (taskZone != null)
        {
            taskZone.onUpgraderChange.RemoveListener(UpdateUI);
        }
    }

    private void UpdateUI()
    {
        taskLabel.text = taskZone.ToTaskZoneString();
    }
}
