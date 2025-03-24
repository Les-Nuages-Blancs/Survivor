using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnerZoneTimeLevelUpgrader : TaskZone
{
    [SerializeField] private float surviveTime = 10f;
    [SerializeField] public UnityEvent onUpgrade;

    private NetworkVariable<float> elapsedTime = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isPaused = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isRunning = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            isRunning.Value = true;
        }

        elapsedTime.OnValueChanged += OnElapsedTimeChanged;
    }

    private void Update()
    {
        if (!IsServer) return; // Only the server updates the timer

        if (!isRunning.Value || isPaused.Value || !LevelStateManager.Instance.SpawnEntity) return;

        elapsedTime.Value += Time.deltaTime;

        if (elapsedTime.Value >= surviveTime)
        {
            elapsedTime.Value = 0f;
            onUpgrade.Invoke();
        }
    }

    public void ResumeBasedOnPresence(bool isHere)
    {
        if (IsServer)
        {
            isPaused.Value = !isHere;
        }
    }

    private void OnElapsedTimeChanged(float oldValue, float newValue)
    {
        // Ensure all clients can react to the time update (e.g., UI updates)
        triggerUpdate();
    }

    public override string ToTaskZoneString()
    {
        return $"Survive {Mathf.Ceil(surviveTime)}s to upgrade zone to next level ({Mathf.Floor(elapsedTime.Value)}s / {Mathf.Ceil(surviveTime)}s)";
    }
}
