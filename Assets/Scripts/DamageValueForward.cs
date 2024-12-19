using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageValueForward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageValueText;

    public void SetDamageValue(float damageValue)
    {
        damageValueText.text = $"-{damageValue}";
    }
}
