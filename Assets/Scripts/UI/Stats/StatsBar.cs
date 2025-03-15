using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image[] rectangles; 
    [SerializeField, Range(0, 5)] private int progress = 0; 
    [SerializeField] private Color activeColor = new Color(145f / 255f, 180f / 255f, 54f / 255f); 


    private void Update()
    {
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        for (int i = 0; i < rectangles.Length; i++)
        {
            rectangles[i].color = (i < progress) ? activeColor  : Color.clear;
        }
    }

    public void SetProgress(int value)
    {
        progress = Mathf.Clamp(value, 0, rectangles.Length);
        UpdateProgressBar();
    }
}
