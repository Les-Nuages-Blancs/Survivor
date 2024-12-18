using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroyAfterTime : NetworkBehaviour
{
    public float maxLifeTime = 5f; // Temps avant destruction

    private void Start()
    {
        if (!IsServer) return;

        StartCoroutine(StartDeathChrono());   
    }

    private IEnumerator StartDeathChrono()
    {
        yield return new WaitForSeconds(maxLifeTime);

        DestroyAfterTimeServerRPC();
    }

    [ServerRpc]
    private void DestroyAfterTimeServerRPC()
    {
        this.GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
