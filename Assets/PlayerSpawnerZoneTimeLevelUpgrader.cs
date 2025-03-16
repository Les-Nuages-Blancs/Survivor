using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class PlayerSpawnerZoneTimeLevelUpgrader : TaskZone
{
    [SerializeField] private float surviveTime = 10;
    [SerializeField] public UnityEvent onUpgrade;

    private float elapsedTime = 0f;
    private bool isPaused = false;
    private float lastTime;
    private bool isRunning = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner) return;
        lastTime = Time.time;
        isRunning = true;
    }

    private void Update()
    {
        if (!isRunning || isPaused || !LevelStateManager.Instance.SpawnEntity) return;

        elapsedTime += Time.deltaTime;
        onUpgraderChange.Invoke();

        if (elapsedTime >= surviveTime)
        {
            onUpgrade.Invoke();
            elapsedTime = 0f;
        }
    }

    public void ResumeBasedOnPresence(bool isHere)
    {
        if (isHere)
        {
            ResumeUpgrade();
        }
        else
        {
            PauseUpgrade();
        }
    }

    public void PauseUpgrade()
    {
        if (isPaused) return;
        isPaused = true;
    }

    public void ResumeUpgrade()
    {
        if (!isPaused) return;
        isPaused = false;
        lastTime = Time.time;
    }

    public override string ToTaskZoneString()
    {
        return $"Survive {Mathf.Ceil(surviveTime)}s to upgrade zone to next level ({Mathf.Floor(elapsedTime)}s / {Mathf.Ceil(surviveTime)}s)";
    }
}