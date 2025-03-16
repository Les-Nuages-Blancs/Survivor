using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class TasksZoneUI : NetworkBehaviour
{
    private ZoneHelper zoneHelper;
    [SerializeField] private GameObject taskZoneUIPrefab;
    [SerializeField] private Transform taskZoneUIPrefabParent;
    [SerializeField] private TextMeshProUGUI zoneTitle;

    private Dictionary<TaskZone, GameObject> taskZoneToPrefab = new Dictionary<TaskZone, GameObject>();

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
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

    public override void OnNetworkSpawn()
    {
        // InitTasksZoneUI();
    }

    private void InitTasksZoneUI()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId; // Get local player's client ID

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient localClient))
        {
            zoneHelper = localClient.PlayerObject.GetComponent<ZoneHelper>();

            if (zoneHelper != null)
            {
                zoneHelper.onZoneChangeDetails.AddListener(onZoneChange);
                AddCallback(zoneHelper.Zone);
            }
            else
            {
                Debug.LogWarning("Local player instance does not have a PlayerController component!");
            }
        }
        else
        {
            Debug.LogWarning("Local player not found in ConnectedClients!");
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
                Debug.Log($"No spawner found for client {localClientId}");
            }
        }
    }

    private void AddCallback(Zone zone)
    {
        if (zone != null)
        {

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
            }

            UpdateTitle(zone);
        }
    }

    private void UpdateTitleWithCurrentZone()
    {
        UpdateTitle(zoneHelper.Zone);
    }

    private void UpdateTitle(Zone zone)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        int spawnerLevel = 0;
        int spawnerMaxLevel = 0;

        if (zone.PlayerSpawners.TryGetValue(localClientId, out SpawnerZone spawner))
        {
            spawnerLevel = spawner.PlayerZoneLevel;
            spawnerMaxLevel = spawner.SpawnerZoneLevelData.levelDatas[0].baseSpawnZoneLevelDatas.Count;
        }
        else
        {
            Debug.Log($"No spawner found for client {localClientId}");
        }

        zoneTitle.text = $"{zone.ZoneName} - Stage {spawnerLevel} / {spawnerMaxLevel}";
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
