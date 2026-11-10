
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float damage = 5f;
    [SerializeField, Min(0.1f)] private float attackInterval = 1f;
    [SerializeField] private float attackRange = 1.5f;

    private float nextAttackTime;

    public float Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;

    public void TryAttack(Transform target)
    {
        if (target == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        // tan cong core
        if (target.TryGetComponent<TurretHealth>(
            out var turretHealth))
        {
            if (turretHealth.IsDead)
                return;

            turretHealth.TakeDamage(damage);
        }
        else if (target.TryGetComponent<CrystalCoreHealth>(
            out var coreHealth))
        {
            if (coreHealth.IsDead)
                return;

            coreHealth.TakeDamage(damage);
        }
        else
        {
            return;
        }

        nextAttackTime = Time.time + attackInterval;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
