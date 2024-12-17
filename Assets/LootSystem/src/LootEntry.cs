using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Flags]
public enum LootCategory
{
    None = 0,
    XP = 1 << 0,        // 1
    RareDrop = 1 << 1,  // 2
    LootEvent = 1 << 2, // 4
    //...
}



/*[System.Flags]
public enum LootEventRequirement
{
    None = 0,          
    Data1 = 1 << 0,    
    Data2 = 1 << 1,
    Data3 = 1 << 2,
    Data4 = 1 << 3, 
    // ...
}*/


public interface ILootEvent
{
    
    void LootResolution();
    void setPlayer(/*Player killer*/);

    //En supposant que plust tard on est énormément de LootEvent différent qui ont besoin de bcp de calcul dans le LootManager :
    
    //renvoit des bitflag qui correspondent à une liste de setters qui devront être effectuer dans le LootManager
    //LootEventRequirement getRequierement();

    //void setAdditionalData1(/*...*/);
    //void setAdditionalData2(/*...*/);
    //void setAdditionalData3(/*...*/);

}

[System.Serializable]
public class LootEntry
{

    [Tooltip("prefab to spawn")]
    public GameObject prefab;
    [Tooltip("Base probability for this object to spawn, in [0:1]")]
    [Range(0f, 1f)]
    public float dropChance;
    [Tooltip("bitflags. Used for specific pre-drop processing in LootManager")]
    public LootCategory category;
}