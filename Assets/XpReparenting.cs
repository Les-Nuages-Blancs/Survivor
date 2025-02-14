using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XpReparenting : NetworkBehaviour
{
    private bool isReparented = false;

    private void Update()
    {
        if (!isReparented && IsSpawned)
        {
            isReparented = true;

            transform.SetParent(LevelStateManager.Instance.XpParent);
        }
    }
}
