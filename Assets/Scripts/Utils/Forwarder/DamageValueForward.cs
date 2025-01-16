using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class DamageValueForward : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI damageValueText;
    public float damageValue;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        SetDamageClientRPC(damageValue);
    }

    [ClientRpc(RequireOwnership = false)]
    private void SetDamageClientRPC(float damage)
    {
        damageValue = damage;
        damageValueText.text = $"-{damageValue}";
    }
}
