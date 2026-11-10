using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretHealth))]
public class TurretCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField, Min(0.05f)] private float attackInterval = 0.2f;
    [SerializeField, Min(0.1f)] private float attackRange = 5f;

    [Header("Target Detection")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Target Priority")]
    [SerializeField] private Transform crystalCore;

    private TurretHealth turretHealth;
    private float nextAttackTime;
    private float attackSpeedPoints;

    private readonly List<EnemyHealth> enemiesInRange = new();

    public EnemyHealth CurrentTarget { get; private set; }
    public float DamagePerTick => damagePerTick;
    public float AttackRange => attackRange;
    public float AttackSpeedPoints => attackSpeedPoints;
    public float AttackInterval => CurrentAttackInterval;
    public float DPS => damagePerTick / CurrentAttackInterval;

    // +5 điểm attack speed nghĩa là bắn nhanh hơn 5% so với tốc độ gốc.
    // Giới hạn interval nhỏ nhất để tránh gây quá nhiều lượt đánh/frame.
    private float CurrentAttackInterval => Mathf.Max(
        0.05f,
        attackInterval / (1f + attackSpeedPoints / 100f));

    private void Awake()
    {
        turretHealth = GetComponent<TurretHealth>();
    }

    private void Update()
    {
        if (turretHealth.IsDead)
            return;

        ScanEnemies();

        // Giữ mục tiêu đến khi chết hoặc ra khỏi tầm đánh.
        if (!IsValidTarget(CurrentTarget))
            ReleaseTarget();

        // Khi cần đổi mục tiêu: chọn Enemy gần Crystal Core nhất.
        if (CurrentTarget == null)
            FindTarget();

        if (CurrentTarget != null)
            Attack();
    }

    private void ScanEnemies()
    {
        enemiesInRange.Clear();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (!IsValidTarget(enemy))
                continue;

            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);
        }
    }

    private bool IsValidTarget(EnemyHealth enemy)
    {
        if (enemy == null || enemy.IsDead)
            return false;

        float distance = Vector2.Distance(
            transform.position, enemy.transform.position);

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

            float distance = (enemy.transform.position
                            - crystalCore.position).sqrMagnitude;

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
        if (CurrentTarget == null || Time.time < nextAttackTime)
            return;

        CurrentTarget.TakeDamage(damagePerTick, turretHealth);

        // Dùng interval sau nâng cấp, không dùng attackInterval gốc.
        nextAttackTime = Time.time + CurrentAttackInterval;
    }

    private void ReleaseTarget()
    {
        CurrentTarget = null;
    }

    public void AddDamage(float amount)
    {
        if (amount <= 0f)
            return;

        damagePerTick += amount;
    }

    public void AddRange(float amount)
    {
        if (amount <= 0f)
            return;

        attackRange += amount;
    }

    public void AddAttackSpeed(float points)
    {
        if (points <= 0f)
            return;

        attackSpeedPoints += points;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}