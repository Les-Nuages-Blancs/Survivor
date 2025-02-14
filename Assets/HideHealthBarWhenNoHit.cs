using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class HideHealthBarWhenNoHit : NetworkBehaviour
{
    [SerializeField] private float noHitTime = 3f;
    [SerializeField] private UnityEvent onHide;
    private Coroutine noHitCoroutine;

    public void RestartNoHitCoroutine()
    {
        if (noHitCoroutine != null)
        {
            StopCoroutine(noHitCoroutine);
        }
        noHitCoroutine = StartCoroutine(NoHitTimer());
    }

    private IEnumerator NoHitTimer()
    {
        yield return new WaitForSeconds(noHitTime);
        HideGameObject();
    }

    private void HideGameObject()
    {
        onHide?.Invoke();
    }
}
