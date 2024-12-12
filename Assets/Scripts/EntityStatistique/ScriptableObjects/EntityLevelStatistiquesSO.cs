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
        return levelStatistiques[levelIndex].Clone();
    }

    public int GetXpRequiredFromLevelToLevel(int startLevel, int endLevel)
    {
        int xpRequired = 0;
        for (int i = startLevel; i <= endLevel; i++)
        {
            xpRequired += levelStatistiques[i].RequiredXpForNextLevel;
        }
        return xpRequired;
    }

    public int GetXpRequiredForNextLevel(int levelIndex)
    {
        return GetXpRequiredFromLevelToLevel(levelIndex, levelIndex);
    }
}
