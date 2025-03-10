using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DoorBlockingPlayer : MonoBehaviour
{
    //private List<MeshCollider> meshColliders;
    private List<StatistiquesLevelSystem> playersStats;
    [SerializeField] private float updtFrequency = 5.0f;
    [SerializeField] private float unlockLvl = 10.0f;

    [SerializeField] private TextMeshProUGUI unlockLabel;

    
    void Awake(){
        playersStats = new List<StatistiquesLevelSystem>();
    }

    public void UpdtPlayerList(){
        playersStats.Clear();
        foreach(StatistiquesLevelSystem stat in LevelStateManager.Instance.PlayerParent.GetComponentsInChildren<StatistiquesLevelSystem>()){
            playersStats.Add(stat);
        }
    }


    float GetAvgPlayerLvl(){
        UpdtPlayerList(); //TODO : update Player List should be a callback whenever a new player connect / disconnect
        float avg = 0;
        foreach(StatistiquesLevelSystem stat in playersStats){
            avg += stat.CurrentLevel;
        }
        return avg /  playersStats.Count;
    }

    void TryToOpen(){
        if(GetAvgPlayerLvl() >= unlockLvl){
            gameObject.SetActive(false);
        }
        else{
            Invoke("TryToOpen", updtFrequency);
        }
    }

    void UpdateLabel()
    {
        unlockLabel.text = "Unlock at Level " + unlockLvl;
    }

    private void OnValidate()
    {
        UpdateLabel();
    }

    // Update is called once per frame
    void Start(){
        UpdateLabel();
        Invoke("TryToOpen", updtFrequency);
    }
}
