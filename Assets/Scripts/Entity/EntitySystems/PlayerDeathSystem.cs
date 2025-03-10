using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDeathSystem : NetworkBehaviour
{
    [SerializeField] private GameObject deathCanvasPrefab;
    [SerializeField] private int respawnCooldownSeconds = 15;
    [SerializeField] private GameObject player;
    
    public void KillPlayer()
    {
        if (!IsOwner) return;
        StartCoroutine(DeathCooldown());
    }

    private IEnumerator DeathCooldown()
    {
        // todo disable player

        var canvas = Instantiate(deathCanvasPrefab, transform.position, Quaternion.identity);
        var deathText = canvas.transform.Find("Respawn Timer").GetComponent<TextMeshProUGUI>();
        for (var i = 0; i < respawnCooldownSeconds; i++)
        {
            deathText.text = "Respawn in: " + (respawnCooldownSeconds - i - 1);
            yield return new WaitForSeconds(1f);
        }
        Destroy(canvas);
        
        // todo enable player
    }
}
