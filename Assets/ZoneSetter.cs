using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSetter : MonoBehaviour
{
    [SerializeField] private Zone zone;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (target != null)
            {
                ZoneHelper zoneHelper = target.GetComponent<ZoneHelper>();

                if (zoneHelper != null)
                {
                    zoneHelper.zone = zone;
                }
            }
        }
    }
}
