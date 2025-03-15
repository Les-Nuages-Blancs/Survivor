using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalPlayerUpgradeManager))]
public class LocalPlayerUpgradeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LocalPlayerUpgradeManager manager = (LocalPlayerUpgradeManager)target;

        if (GUILayout.Button("Upgrade Health"))
        {
            manager.UpgradeHealth();
        }

        if (GUILayout.Button("Upgrade Health Regen"))
        {
            manager.UpgradeHealthRegen();
        }

        if (GUILayout.Button("Upgrade Armor"))
        {
            manager.UpgradeArmor();
        }

        if (GUILayout.Button("Upgrade Damage"))
        {
            manager.UpgradeDamage();
        }

        if (GUILayout.Button("Upgrade Attack Speed"))
        {
            manager.UpgradeAttackSpeed();
        }

        if (GUILayout.Button("Upgrade Pickup Range"))
        {
            manager.UpgradePickupRange();
        }

        if (GUILayout.Button("Upgrade Move Speed Multiplier"))
        {
            manager.UpgradeMoveSpeedMultiplier();
        }
    }
}
