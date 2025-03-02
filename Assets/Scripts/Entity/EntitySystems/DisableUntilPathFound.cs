using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



/*
The goal of this script is to disable ennemy until they found a path.

path computation seems to be a bottleneck

When there are ~100 enemy, and a new on spawn, the path computation for newly spawned ennemy will occur last in the queue.
Meaning that in stress condition, ennemy can spawn and stay still for like 1s before finding a path.

This script hide the standing still enemy.
*/

public class DisableUntiPathFound : MonoBehaviour
{

    private bool hasFoundFirstPath = false;
    private NavMeshAgent agent;
    [Tooltip("the frequency at which ennemy will try to exist if it has found it's first path. Lower value = slightly better perf / smaller valuer => just use the update callback")]
    [SerializeField] float updtFrequency = 0.2f;


    void SetChildrenActive(bool v){
        foreach(Transform child in transform){
            child.gameObject.SetActive(v);
        }
    }

    void TryToExist(){
        if(agent.pathPending){
            Invoke("TryToExist", updtFrequency);
        } else{
            SetChildrenActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetChildrenActive(false);
        Invoke("TryToExist", updtFrequency);
    }

}