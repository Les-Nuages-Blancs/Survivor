using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Camera;
using Unity.Netcode;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDeathSystem : NetworkBehaviour
{
    [SerializeField] private GameObject deathCanvasPrefab;
    [SerializeField] private int respawnCooldownSeconds = 15;
    
    [SerializeField] private MonoBehaviour[] scripts;
    [Tooltip("below function that will be called on death and on revive. Note that is different from scripts above that are disabled / enabled. The reason is that it may cause desyncronisation issues should we disabled script wwith long cooldown over time")]
    [SerializeField] private UnityEvent[] TogglesOnDeath;
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
        foreach(UnityEvent e in TogglesOnDeath){
            e?.Invoke();
        }

        foreach (var playerRenderer in playerRenderers)
        {
            playerRenderer.enabled = false;
        }
        transform.position = applyPlayerSpawn.spawnPosition;
        yield return new WaitForSeconds(0.1f);

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
        foreach(UnityEvent e in TogglesOnDeath){
            e?.Invoke();
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
        IsDead = false;

    }
}
