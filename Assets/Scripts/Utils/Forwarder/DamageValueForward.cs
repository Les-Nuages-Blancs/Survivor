using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DamageValueForward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageValueText;
    public float damageValue;
    
    private void Start() {
        damageValueText.text = $"-{damageValue}";
    }
}
