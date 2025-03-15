using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public enum UpgradeType
    {
        Health,
        HealthRegen,
        Armor,
        Damage,
        AttackSpeed,
        PickupRange,
        MoveSpeedMultiplier
    }

    [SerializeField] private Image[] rectangles;
    [SerializeField, Range(0, 5)] private int progress = 0;
    [SerializeField] private Color activeColor = new Color(145f / 255f, 180f / 255f, 54f / 255f);
    [SerializeField] private UpgradeType upgradeType;

    private void Start()
    {
        LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.AddListener(OnUpgraderChange);
        UpdateProgressBasedOnLocalPlayer();
    }

    private void OnDestroy()
    {
        LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.RemoveListener(OnUpgraderChange);
    }

    private void OnUpgraderChange(EntityBaseStatistiques mult, EntityBaseStatistiques add)
    {
        UpdateProgressBasedOnLocalPlayer();
    }

    private void UpdateProgressBasedOnLocalPlayer()
    {
        EntityBaseStatistiques upgraderLevel = LocalPlayerUpgradeManager.Instance.StatsUpgraderLevel;

        int newProgress = progress;
        switch (upgradeType)
        {
            case UpgradeType.Health:
                newProgress = (int)upgraderLevel.Health;
                break;
            case UpgradeType.HealthRegen:
                newProgress = (int)upgraderLevel.RegenHealth;
                break;
            case UpgradeType.Armor:
                newProgress = (int)upgraderLevel.Armor;
                break;
            case UpgradeType.Damage:
                newProgress = (int)upgraderLevel.Damage;
                break;
            case UpgradeType.AttackSpeed:
                newProgress = (int)upgraderLevel.AttackSpeed;
                break;
            case UpgradeType.PickupRange:
                newProgress = (int)upgraderLevel.PickupRange;
                break;
            case UpgradeType.MoveSpeedMultiplier:
                newProgress = (int)upgraderLevel.MoveSpeedMultiplier;
                break;
        }

        SetProgress(newProgress);
    }

    private void Update()
    {
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        for (int i = 0; i < rectangles.Length; i++)
        {
            rectangles[i].color = (i < progress) ? activeColor : Color.clear;
        }
    }

    public void SetProgress(int value)
    {
        progress = Mathf.Clamp(value, 0, rectangles.Length);
        UpdateProgressBar();
    }
}
