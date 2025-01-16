using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statLevelSystem;
    [SerializeField] private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    private void Start()
    {
        SetLevel();
    }

    public void SetLevel()
    {
        text.text = statLevelSystem.CurrentLevel.ToString();
    }
}
