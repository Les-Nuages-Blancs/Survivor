using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] protected GameObject prefab;

    [SerializeField] protected float spawnCooldown = 2.0f;

    [SerializeField] public UnityEvent OnWillSpawn;

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        StartSpawn();
    }

    private void OnDisable()
    {
        StopSpawn();
    }


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (isActiveAndEnabled)
        {
            StartSpawn();
        }
    }

    private void StartSpawn()
    {
        if (!IsServer) return;
        spawnCoroutine = StartCoroutine(SpawnPrefabCoroutine());
    }

    private void StopSpawn()
    {
        if (!IsServer) return;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    private IEnumerator SpawnPrefabCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);

            SpawnPrefabServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPrefabServerRPC()
    {
        SpawnPrefabServer();
    }

    protected virtual GameObject SpawnPrefabServer()
    {
        OnWillSpawn.Invoke();

        GameObject go = Instantiate(prefab, transform.position, transform.rotation);
        go.GetComponent<NetworkObject>().Spawn();

        return go;
    }
}
