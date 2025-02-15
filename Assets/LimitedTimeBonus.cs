using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LimitedTimeBonus : NetworkBehaviour
{
    [SerializeField] private float limitedTime = 10.0f;

    [SerializeField] private UnityEvent OnBonusStart;
    [SerializeField] private UnityEvent OnBonusStop;

    private Coroutine bonusCoroutine;

    public void StartBonus()
    {
        if (!IsServer) return;

        if (bonusCoroutine != null)
        {
            StopCoroutine(bonusCoroutine);
        }
        bonusCoroutine = StartCoroutine(LaunchBonusCoroutine());

        onBonusStart();
        OnBonusStart.Invoke();
    }

    public void StopBonus()
    {
        if (!IsServer) return;

        if (bonusCoroutine != null)
        {
            StopCoroutine(bonusCoroutine);
        }

        onBonusStop();
        OnBonusStop.Invoke();
    }

    private IEnumerator LaunchBonusCoroutine()
    {
        yield return new WaitForSeconds(limitedTime);

        StopBonus();
    }

    virtual protected void onBonusStart()
    {

    }

    virtual protected void onBonusStop()
    {

    }
}
