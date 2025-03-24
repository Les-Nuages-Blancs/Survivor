using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TasksZoneUI : NetworkBehaviour
{
    private ZoneHelper zoneHelper;
    [SerializeField] private GameObject taskZoneUIPrefab;
    [SerializeField] private Transform taskZoneUIPrefabParent;
    [SerializeField] private TextMeshProUGUI zoneTitle;

    private Dictionary<TaskZone, GameObject> taskZoneToPrefab = new Dictionary<TaskZone, GameObject>();

    public ZoneHelper ZoneHelper
    {
        get => zoneHelper;
        set { 
            ZoneHelper previousZone = zoneHelper;
            zoneHelper = value;
            if (previousZone == null && zoneHelper != null)
            {
                InitCallback();
            }
        }
    }

    public void Init()
    {
        foreach (Player player in Player.playerList)
        {
            OnClientConnected(player.OwnerClientId);
        }
        Player.onPlayerAdded += OnClientConnected;
    }

    private void InitCallback()
    {
        if (zoneHelper == null)
        {
            Debug.LogWarning("zoneHelper is null");
        }
        else
        {
            AddCallback(zoneHelper.Zone);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected!");

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Initializing Task Zone UI for local client...");
            InitTasksZoneUI();
        }
    }

    private void InitTasksZoneUI()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId; // Get local player's client ID

        Player localPlayer = Player.GetPlayerByClientId(localClientId);
        
        zoneHelper = localPlayer.GetComponent<ZoneHelper>();

        if (zoneHelper != null)
        {
            zoneHelper.onZoneChangeDetails.AddListener(onZoneChange);
            UpdateTitleWithCurrentZone();
        }
        else
        {
            Debug.LogWarning("Local player instance does not have a PlayerController component!");
        }
    }

    public override void OnNetworkDespawn()
    {
        RemoveCallback(zoneHelper.Zone);
        zoneHelper.onZoneChangeDetails.RemoveListener(onZoneChange);
    }

    private void onZoneChange(Zone oldZone, Zone newZone)
    {
        RemoveCallback(oldZone);
        AddCallback(newZone);
    }

    private void RemoveCallback(Zone zone)
    {
        if (zone != null)
        {
            Debug.Log("tasks remove callback");

            zone.OnTaskAdded.RemoveListener(onTaskAdded);
            zone.OnTaskRemove.RemoveListener(onTaskRemove);

            foreach (TaskZone taskZone in zone.ZoneTasks)
            {
                onTaskRemove(taskZone);
            }

            ulong localClientId = NetworkManager.Singleton.LocalClientId;

            if (zone.PlayerSpawners.TryGetValue(localClientId, out SpawnerZone spawner))
            {
                spawner.onPlayerZoneLevelChange.RemoveListener(UpdateTitleWithCurrentZone);
            }
            else
            {
                zone.OnEnemySpawnerAdded.RemoveListener(tryListenZoneLevelChange);
                Debug.Log($"No spawner found for client {localClientId}");
            }
        }
    }

    private void AddCallback(Zone zone)
    {
        if (zone != null)
        {
            Debug.Log("tasks add callback");
            zone.OnTaskAdded.AddListener(onTaskAdded);
            zone.OnTaskRemove.AddListener(onTaskRemove);

            foreach (TaskZone taskZone in zone.ZoneTasks)
            {
                onTaskAdded(taskZone);
            }

            ulong localClientId = NetworkManager.Singleton.LocalClientId;

            if (zone.PlayerSpawners.TryGetValue(localClientId, out SpawnerZone spawner))
            {
                // TODO fix start zone that throw warning instead of finding the local client id spawner
                spawner.onPlayerZoneLevelChange.AddListener(UpdateTitleWithCurrentZone);
            }
            else
            {
                Debug.Log($"No spawner found for client {localClientId}");
                zone.OnEnemySpawnerAdded.AddListener(tryListenZoneLevelChange);
            }

            UpdateTitle(zone);
        }
    }

    private void tryListenZoneLevelChange(ulong clientId)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (clientId == localClientId)
        {
            zoneHelper.Zone.OnEnemySpawnerAdded.RemoveListener(tryListenZoneLevelChange);

            if (zoneHelper.Zone.PlayerSpawners.TryGetValue(localClientId, out SpawnerZone spawner))
            {
                // TODO fix start zone that throw warning instead of finding the local client id spawner
                spawner.onPlayerZoneLevelChange.AddListener(UpdateTitleWithCurrentZone);
            }
        }
    }

    private void UpdateTitleWithCurrentZone()
    {
        UpdateTitle(zoneHelper.Zone);
    }

    private void UpdateTitle(Zone zone)
    {
        if (zone != null)
        {
            ulong localClientId = NetworkManager.Singleton.LocalClientId;

            int spawnerLevel = 0;
            int spawnerMaxLevel = 0;

            if (zone.PlayerSpawners.TryGetValue(localClientId, out SpawnerZone spawner))
            {
                spawnerLevel = spawner.PlayerZoneLevel;
                spawnerMaxLevel = spawner.SpawnerZoneLevelData.levelDatas[0].baseSpawnZoneLevelDatas.Count - 1;
            }
            else
            {
                Debug.Log($"No spawner found for client {localClientId}");
            }

            zoneTitle.text = $"{zone.ZoneName}" + (spawnerMaxLevel == 0 ? "" : $" - Stage {spawnerLevel} / {spawnerMaxLevel}");
        }
        else
        {
            Debug.LogWarning($"Zone is null atm");
        }
    }

    private void onTaskAdded(TaskZone taskZone)
    {
        if (taskZone == null || taskZoneToPrefab.ContainsKey(taskZone)) return;

        GameObject taskZoneUIGo = Instantiate(taskZoneUIPrefab, taskZoneUIPrefabParent);
        taskZoneToPrefab[taskZone] = taskZoneUIGo;

        TaskZoneUI tasksZoneUI = taskZoneUIGo.GetComponent<TaskZoneUI>();
        tasksZoneUI.InitTaskZone(taskZone);
    }

    private void onTaskRemove(TaskZone taskZone)
    {
        if (taskZone == null || !taskZoneToPrefab.ContainsKey(taskZone)) return;

        GameObject taskZoneUIGo = taskZoneToPrefab[taskZone];
        Destroy(taskZoneUIGo);
        taskZoneToPrefab.Remove(taskZone);
    }
}
