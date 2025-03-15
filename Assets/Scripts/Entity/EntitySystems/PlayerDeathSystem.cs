using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Camera;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDeathSystem : NetworkBehaviour
{
    [SerializeField] private GameObject deathCanvasPrefab;
    [SerializeField] private int respawnCooldownSeconds = 15;
    
    [SerializeField] private MonoBehaviour[] scripts;
    [SerializeField] private Renderer[] playerRenderers;
    [SerializeField] private Collider[] playerColliders;
    [SerializeField] private Canvas[] playerCanvases;
    
    [SerializeField] private ApplyPlayerSpawn applyPlayerSpawn;
    [SerializeField] private HealthSystem healthSystem;

    public bool IsDead { get; private set; } = false;

    public void KillPlayer()
    {
        if (!IsOwner || IsDead) return;
        Camera.main.gameObject.GetComponent<CameraFollowSystem>().StartCoroutine(DeathCooldown());
    }

    private IEnumerator DeathCooldown()
    {
        IsDead = true;

        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        foreach (var playerRenderer in playerRenderers)
        {
            playerRenderer.enabled = false;
        }

        foreach (var playerCollider in playerColliders)
        {
            playerCollider.enabled = false;
        }

        foreach (var playerCanvas in playerCanvases)
        {
            playerCanvas.enabled = false;
        }
        
        var canvas = Instantiate(deathCanvasPrefab, transform.position, Quaternion.identity);
        var deathText = canvas.transform.Find("Respawn Timer").GetComponent<TextMeshProUGUI>();
        
        for (var i = 0; i < respawnCooldownSeconds; i++)
        {
            deathText.text = "Respawn in: " + (respawnCooldownSeconds - i - 1);
            yield return new WaitForSeconds(1f);
        }
        Destroy(canvas);
        
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
        foreach (var playerRenderer in playerRenderers)
        {
            playerRenderer.enabled = true;
        }
        foreach (var playerCanvas in playerCanvases)
        {
            playerCanvas.enabled = true;
        }
        foreach (var playerCollider in playerColliders)
        {
            playerCollider.enabled = true;
        }
        healthSystem.CurrentHealth = healthSystem.MaxHealth;
        transform.position = applyPlayerSpawn.spawnPosition;
        IsDead = false;

    }
}
