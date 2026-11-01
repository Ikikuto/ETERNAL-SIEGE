using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretHealth))]
[RequireComponent(typeof(CircleCollider2D))]

public class TurretCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField, Min(0.05f)] private float attackInterval = 0.2f;
    [SerializeField, Min(0.1f)] private float attackRange = 5f;
    private TurretHealth turretHealth;
    private CircleCollider2D rangeTrigger;
    private readonly List<EnemyHealth> enemiesInRange = new();
    private float nextAttackTime;
    public EnemyHealth CurrentTarget { get; private set; }
    public float DamagePerTick => damagePerTick;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;
    public float DPS => damagePerTick / attackInterval;

    private void Awake()
    {
        turretHealth = GetComponent<TurretHealth>();
        rangeTrigger = GetComponent<CircleCollider2D>();

        // Collider phat hien enemy
        rangeTrigger.isTrigger = true;
        rangeTrigger.offset = Vector2.zero;
        rangeTrigger.radius = attackRange;
    }

    // turret is dead >> return
    // Check current target (IsValidTarget) >> release target
    // Tim new target
    // tan cong muc tieu
    private void Update()
    {
        if (turretHealth.IsDead) return;

        if (!IsValidTarget(CurrentTarget)) ReleaseTarget();

        if (CurrentTarget == null) FindTarget();

        if (CurrentTarget != null) Attack();
    }

    // check turret IsDead
    // get component EnemyHealth khi enemy trigger
    // check enemy null or IsDead
    // add enemy vao list enemiesInRange
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (turretHealth.IsDead)
            return;

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy == null || enemy.IsDead)
            return;

        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    // get component EnemyHealth khi enemy exit trigger
    // check enemy null
    // remove enemy khoi list enemiesInRange
    // gan null cho CurrentTarget neu exit trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        enemiesInRange.Remove(enemy);

        if (CurrentTarget == enemy)
        {
            ReleaseTarget();
        }
    }

    // Xet trong vong lap for (start tu 0)
    // Xoa enemy null hoac IsDead khoi list
    // check IsValidTarget
    // lock first valid enemy
    private void FindTarget()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            EnemyHealth enemy = enemiesInRange[i];

            if (enemy == null || enemy.IsDead)
            {
                enemiesInRange.RemoveAt(i);
                i--;
                continue;
            }

            if (!IsValidTarget(enemy))
                continue;

            CurrentTarget = enemy;
            return;
        }
    }

    // EnemyHealth: null, enemy dead, enemy out of range
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

    // check nextAttackTime
    // enemy xac dinh first turret gay damage (TakeDamage)
    // nextAttackTime = Time.time + attackInterval
    private void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        // Truyền Turret gây damage cho Enemy
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

    // POWER-UP: Sharp Bullet
    // public void IncreaseDamage(float amount)
    // {
    //     if (amount <= 0f)
    //         return;

    //     damagePerTick += amount;
    // }

    // POWER-UP: Rapid Fire
    // public void IncreaseAttackSpeed(float percent)
    // {
    //     if (percent <= 0f)
    //         return;

    //     attackInterval = Mathf.Max(
    //         0.05f,
    //         attackInterval / (1f + percent)
    //     );
    // }

    // POWER-UP: Long Range
    // public void IncreaseRange(float amount)
    // {
    //     if (amount <= 0f)
    //         return;

    //     attackRange += amount;
    //     rangeTrigger.radius = attackRange;
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
