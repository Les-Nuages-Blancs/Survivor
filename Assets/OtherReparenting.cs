using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OtherReparenting : NetworkBehaviour
{
    private bool isReparented = false;

    private void Update()
    {
        if (!isReparented && IsSpawned)
        {
            isReparented = true;

            transform.SetParent(LevelStateManager.Instance.OtherParent);
        }
    }
}
