using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
The point of this script is solely to disable geometry used for navmesh
*/
public class DisableChildObjectOnStart : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
