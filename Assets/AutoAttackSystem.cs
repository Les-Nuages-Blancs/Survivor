using UnityEngine;
using System.Collections;

public class AutoAttackSystem : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float attackSpeed;

    private Coroutine attackCoroutine;

    private void Start()
    {
        UpdateAttackStats();
    }

    public void UpdateAttackStats()
    {
        attackSpeed = statsLevelSystem.BaseStatistiques.AttackSpeed;
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateAttackStats();
        }
    }

    private void OnEnable()
    {
        attackCoroutine = StartCoroutine(LaunchAttack());
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / attackSpeed);
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }
}