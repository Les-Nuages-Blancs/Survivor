using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class StatistiquesLevelSystem : NetworkBehaviour
{
    [SerializeField] private EntityLevelStatistiquesSO entityLevelStatistiques;

    [SerializeField] private NetworkVariable<int> currentLevel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> currentXp = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private NetworkVariable<EntityBaseStatistiques> baseStatistiques = new NetworkVariable<EntityBaseStatistiques>(
        new EntityBaseStatistiques(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<EntityBaseStatistiques> baseStatistiquesMultiplier = new NetworkVariable<EntityBaseStatistiques>(
        new EntityBaseStatistiques(1, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<EntityBaseStatistiques> baseStatistiquesAdditiveBonus = new NetworkVariable<EntityBaseStatistiques>(
        new EntityBaseStatistiques(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] public UnityEvent onBaseStatsChange;
    [SerializeField] public UnityEvent onBaseStatistiquesMultiplierChange;
    [SerializeField] public UnityEvent onBaseStatistiquesAdditiveBonusChange;
    [SerializeField] public UnityEvent onCurrentStatistiquesChange;

    [SerializeField] public UnityEvent onCurrentLevelChange;
    [SerializeField] public UnityEvent onCurrentXpChange;

    public EntityBaseStatistiques BaseStatistiques => baseStatistiques.Value;
    public EntityBaseStatistiques BaseStatistiquesMultiplier => baseStatistiquesMultiplier.Value;
    public EntityBaseStatistiques BaseStatistiquesAdditiveBonus => baseStatistiquesAdditiveBonus.Value;
    public EntityBaseStatistiques CurrentStatistiques => (baseStatistiques.Value + baseStatistiquesAdditiveBonus.Value) * baseStatistiquesMultiplier.Value;
    public EntityLevelStatistiquesSO EntityLevelStatistiques => entityLevelStatistiques;

    public int CurrentLevel
    {
        get => currentLevel.Value;
        set
        {
            if (currentLevel.Value != value)
            {
                currentLevel.Value = value;
                UpdateLevelStat();
            }
        }
    }

    public int CurrentXp
    {
        get => currentXp.Value;
        set
        {
            if (currentXp.Value != value)
            {
                currentXp.Value = value;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        baseStatistiques.OnValueChanged += OnBaseStatsChange;
        baseStatistiquesAdditiveBonus.OnValueChanged += OnBaseStatsAdditiveBonusChange;
        baseStatistiquesMultiplier.OnValueChanged += OnBaseStatsMultiplierChange;
        currentLevel.OnValueChanged += OnCurrentLevelChange;
        currentXp.OnValueChanged += OnCurrentXpChange;

        if (IsOwner && GetComponent<Player>() != null)
        {
            UpdateUpgrader(LocalPlayerUpgradeManager.Instance.GetCurrentStatsMultiplier(), LocalPlayerUpgradeManager.Instance.GetCurrentStatsAdditiveBonus());
            LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.AddListener(UpdateUpgrader);
        }

        UpdateLevelStat();
        TryLevelUp();
    }

    private void UpdateUpgrader(EntityBaseStatistiques newMultiplier, EntityBaseStatistiques newAdditiveBonus)
    {
        baseStatistiquesMultiplier.Value = newMultiplier;
        baseStatistiquesAdditiveBonus.Value = newAdditiveBonus;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        baseStatistiques.OnValueChanged -= OnBaseStatsChange;
        baseStatistiquesAdditiveBonus.OnValueChanged -= OnBaseStatsAdditiveBonusChange;
        baseStatistiquesMultiplier.OnValueChanged -= OnBaseStatsMultiplierChange;
        currentLevel.OnValueChanged -= OnCurrentLevelChange;
        currentXp.OnValueChanged -= OnCurrentXpChange;

        if (IsOwner)
        {
            LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.RemoveListener(UpdateUpgrader);
        }
    }

    private void OnBaseStatsChange(EntityBaseStatistiques oldValue, EntityBaseStatistiques newValue)
    {
        onBaseStatsChange.Invoke();
        onCurrentStatistiquesChange.Invoke();
        // TODO see why changing health from health system update base stats
        // Debug.Log("base stats change - " + gameObject.name);
    }

    private void OnBaseStatsAdditiveBonusChange(EntityBaseStatistiques oldValue, EntityBaseStatistiques newValue)
    {
        onBaseStatistiquesAdditiveBonusChange.Invoke();
        onCurrentStatistiquesChange.Invoke();
    }

    private void OnBaseStatsMultiplierChange(EntityBaseStatistiques oldValue, EntityBaseStatistiques newValue)
    {
        onBaseStatistiquesMultiplierChange.Invoke();
        onCurrentStatistiquesChange.Invoke();
    }

    private void OnCurrentLevelChange(int oldValue, int newValue)
    {
        onCurrentLevelChange.Invoke();
    }

    private void OnCurrentXpChange(int oldValue, int newValue)
    {
        onCurrentXpChange.Invoke();
    }

    private void UpdateLevelStat()
    {
        if (!IsServer) return;

        EntityBaseStatistiques stats = entityLevelStatistiques.GetStatsOfLevel(CurrentLevel);

        if (!stats.Equals(baseStatistiques.Value))
        {
            baseStatistiques.Value = stats;
        }
    }

    private void TryLevelUp()
    {
        if (!IsServer) return;
        TradeXpForLevelServerRPC(-1);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddLevelAndResetXpServerRPC(int level)
    {
        CurrentLevel += level;
        CurrentXp = 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetLevelServerRPC(int level)
    {
        CurrentLevel = level;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddLevelServerRPC(int level)
    {
        CurrentLevel += level;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TradeXpForLevelServerRPC(int level)
    {
        if (level == 0) return;
        int levelUpXpCost = entityLevelStatistiques.GetXpRequiredForNextLevel(CurrentLevel);
        if (CurrentXp >= levelUpXpCost)
        {
            CurrentXp -= levelUpXpCost;
            CurrentLevel += 1;
            if (level - 1 != 0)
            {
                TradeXpForLevelServerRPC(level - 1);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetXpServerRPC(int xp)
    {
        CurrentXp = xp;
        TradeXpForLevelServerRPC(-1); // auto convert xp into level when possible
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddXpServerRPC(int xp)
    {
        CurrentXp += xp;
        TradeXpForLevelServerRPC(-1); // auto convert xp into level when possible
    }
}
