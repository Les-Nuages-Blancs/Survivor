using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class Card
{
    public enum CardType { Armor, Damage, Health, AttackSpeed, HealthRegen, Railgun, LavaFlask }

    public string title;
    public Sprite image;
    public CardType type;

    public void SetTitleAndImage(string title, Sprite image)
    {
        this.title = title;
        this.image = image;
    }

    public void SetCardType(CardType type)
    {
        this.type = type;
    }

    public void ApplyEffect()
    {
        if (LocalPlayerUpgradeManager.Instance == null)
        {
            Debug.LogError("LocalPlayerUpgradeManager instance is missing!");
            return;
        }

        switch (type)
        {
            case CardType.Armor:
                LocalPlayerUpgradeManager.Instance.UpgradeArmor();
                break;
            case CardType.Damage:
                LocalPlayerUpgradeManager.Instance.UpgradeDamage();
                break;
            case CardType.Health:
                LocalPlayerUpgradeManager.Instance.UpgradeHealth();
                break;
            case CardType.AttackSpeed:
                LocalPlayerUpgradeManager.Instance.UpgradeAttackSpeed();
                break;
            case CardType.HealthRegen:
                LocalPlayerUpgradeManager.Instance.UpgradeHealthRegen();
                break;
            case CardType.Railgun:
                LocalPlayerUpgradeManager.Instance.railgunLvl++;
                LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.Invoke(LocalPlayerUpgradeManager.Instance.GetCurrentStatsMultiplier(), LocalPlayerUpgradeManager.Instance.GetCurrentStatsAdditiveBonus());
                GameNetworkManager.Instance.LvlUpRailgunSystemServerRpc(NetworkManager.Singleton.LocalClientId);
                break;
            case CardType.LavaFlask:
                LocalPlayerUpgradeManager.Instance.lavaflaskLvl++;
                LocalPlayerUpgradeManager.Instance.onStatsUpgraderLevelChange.Invoke(LocalPlayerUpgradeManager.Instance.GetCurrentStatsMultiplier(), LocalPlayerUpgradeManager.Instance.GetCurrentStatsAdditiveBonus());
                GameNetworkManager.Instance.LvlUpLavaFlaskSystemServerRpc(NetworkManager.Singleton.LocalClientId);
                break;
            default:
                Debug.LogWarning("Unknown card type!");
                break;
        }
    }

}
