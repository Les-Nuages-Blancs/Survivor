using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;
    [Tooltip("The LootTable to be processed when calling the processLootTable function.")]
    [SerializeField] private LootTable lootTable;
    [Tooltip("Will the game object be destoyed after processing loot ?")]
    [SerializeField] private bool destroyOnProcessLoot = true;



    //TODO FIXME TORM this Start callback should be removed in release mode. This warning is left here for know for easier debugging purposes.
    public void Start(){
        if(lootTable == null){
            Debug.LogError("Error : No LootTable associated with this LootSystem !");
        }
    }
    public void processLootTable(){
        LootManager.Instance.ProcessLootTable(lootTable, (targetGameObject == null ? gameObject : targetGameObject), transform);
        
        //If performance ever become an issue consider removing conditional statement from this script. There are several other ways to acheive one LootTable looting multiples times without destroying any gameObject. Notably, use directly the LootManager singleton for the best performance. The whole "system" architecture is made for us to be convenient to work with in the Unity editor
        if(destroyOnProcessLoot) Destroy(gameObject);
    }

}
