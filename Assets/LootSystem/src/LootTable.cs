using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Loot System/Loot Table")]
public class LootTable : ScriptableObject
{
    [Tooltip("List of potential loot")]
    public List<LootEntry> lootEntries;
}
