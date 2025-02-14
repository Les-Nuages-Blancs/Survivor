using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityLevelStatistiques", menuName = "ScriptableObject/EntityLevelStatistiques")]
public class EntityLevelStatistiquesSO : ScriptableObject
{

    [LevelsStats]
    [SerializeField] public List<EntityBaseStatistiques> levelStatistiques = new List<EntityBaseStatistiques>();

    public EntityBaseStatistiques GetStatsOfLevel(int levelIndex)
    {
        if (levelStatistiques.Count > levelIndex)
        {
            return levelStatistiques[levelIndex].Clone();
        }
        else
        {
            return levelStatistiques[levelStatistiques.Count - 1];
        }
    }

    public int GetXpRequiredFromLevelToLevel(int startLevel, int endLevel)
    {
        int xpRequired = 0;
        for (int i = startLevel; i <= endLevel; i++)
        {
            if (levelStatistiques.Count > i)
            {
                xpRequired += levelStatistiques[i].RequiredXpForNextLevel;
            }
        }
        return xpRequired;
    }

    public int GetXpRequiredForNextLevel(int levelIndex)
    {
        return GetXpRequiredFromLevelToLevel(levelIndex, levelIndex);
    }
}
