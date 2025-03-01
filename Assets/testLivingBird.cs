using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLivingBird : MonoBehaviour
{
    public lb_BirdController birdControl;
    public int spawnAmount = 10;

    Camera currentCamera;
    Ray ray;
    RaycastHit[] hits;

    void Start()
    {
        currentCamera = Camera.main;
        birdControl = GameObject.Find("_livingBirdsController").GetComponent<lb_BirdController>();
        birdControl.SendMessage("ChangeCamera", currentCamera);
        SpawnSomeBirds();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "lb_bird")
                {
                    hit.transform.SendMessage("KillBirdWithForce", ray.direction * 500);
                    break;
                }
            }
        }
    }

    void OnGUI()
    {
        int offset = 150;
        if (GUI.Button(new Rect(10, 10 + offset, 150, 50), "Pause"))
            birdControl.SendMessage("Pause");

        if (GUI.Button(new Rect(10, 70 + offset, 150, 30), "Scare All"))
            birdControl.SendMessage("AllFlee");

        if (GUI.Button(new Rect(10, 170 + offset, 150, 50), "Revive Birds"))
            birdControl.BroadcastMessage("Revive");
    }

    IEnumerator SpawnSomeBirds()
    {
        yield return 2;
        birdControl.SendMessage("SpawnAmount", spawnAmount);
    }
}
