using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretHealth))]
public class TurretCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField, Min(0.05f)]
    private float attackInterval = 0.2f;
    [SerializeField, Min(0.1f)]
    private float attackRange = 5f;

    [Header("Target Detection")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Target Priority")]
    [SerializeField] private Transform crystalCore;

    private TurretHealth turretHealth;
    private float nextAttackTime;

    // danh sach Enemy (dectect theo thu tu)
    private readonly List<EnemyHealth> enemiesInRange = new();

    public EnemyHealth CurrentTarget { get; private set; }

    public float DamagePerTick => damagePerTick;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;
    public float DPS => damagePerTick / attackInterval;

    private void Awake()
    {
        turretHealth = GetComponent<TurretHealth>();
    }

    private void Update()
    {
        if (turretHealth.IsDead)
            return;

        ScanEnemies();

        if (!IsValidTarget(CurrentTarget))
        {
            ReleaseTarget();
        }

        if (CurrentTarget == null)
        {
            FindTarget();
        }

        if (CurrentTarget != null)
        {
            Attack();
        }
    }

    private void ScanEnemies()
    {
        enemiesInRange.Clear();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (!IsValidTarget(enemy))
                continue;

            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private bool IsValidTarget(EnemyHealth enemy)
    {
        if (enemy == null || enemy.IsDead)
            return false;

        float distance = Vector2.Distance(
            transform.position,
            enemy.transform.position
        );

        return distance <= attackRange;
    }


    private void FindTarget()
    {
        if (crystalCore == null)
            return;

        EnemyHealth nearestCoreEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyHealth enemy in enemiesInRange)
        {
            if (!IsValidTarget(enemy))
                continue;

            float distance = (
                enemy.transform.position -
                crystalCore.position
            ).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCoreEnemy = enemy;
            }
        }

        CurrentTarget = nearestCoreEnemy;
    }

    private void Attack()
    {
        if (CurrentTarget == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        CurrentTarget.TakeDamage(
            damagePerTick,
            turretHealth
        );

        nextAttackTime = Time.time + attackInterval;
    }

    private void ReleaseTarget()
    {
        CurrentTarget = null;
    }

    // Sharp Bullet
    // public void IncreaseDamage(float amount)
    // {
    //     if (amount <= 0f)
    //         return;

    //     damagePerTick += amount;
    // }

    // Rapid Fire
    // public void IncreaseAttackSpeed(float percent)
    // {
    //     if (percent <= 0f)
    //         return;

    //     attackInterval = Mathf.Max(
    //         0.05f,
    //         attackInterval / (1f + percent)
    //     );
    // }

    // Long Range
    // public void IncreaseRange(float amount)
    // {
    //     if (amount <= 0f)
    //         return;

    //     attackRange += amount;
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}
