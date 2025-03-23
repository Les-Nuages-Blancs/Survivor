using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificPlayerInfoUI : MonoBehaviour
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private XpBarUI xpBarUI;
    [SerializeField] private LvlUI lvlUI;

    public void Setup(ulong clientId)
    {
        healthBarUI.Setup(clientId);
        xpBarUI.Setup(clientId);
        lvlUI.Setup(clientId);
    }
}
