using UnityEngine;

public enum EnemyState
{
    MovingToCore,
    ChasingTurret,
    Attacking,
    Dead
}

[RequireComponent(
    typeof(EnemyMovement),
    typeof(EnemyCombat),
    typeof(EnemyHealth)
)]
public class EnemyAI : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyCombat combat;
    private EnemyHealth health;
    private CrystalCoreHealth crystalCore;
    private TurretHealth aggroTurret;
    private Transform currentTarget;

    public EnemyState CurrentState { get; private set; }
    public bool IsDead => health.IsDead;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        health = GetComponent<EnemyHealth>();
    }

    public void Initialize(CrystalCoreHealth core)
    {
        crystalCore = core;
        CurrentState = EnemyState.MovingToCore;
    }

    private void Update()
    {
        if (health.IsDead) return;

        UpdateTarget();

        if (currentTarget == null)
        {
            movement.Stop();
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            currentTarget.position
        );

        if (distance <= combat.AttackRange)
        {
            movement.SetMovementEnabled(false);
            CurrentState = EnemyState.Attacking;

            combat.TryAttack(currentTarget);
        }
        else
        {
            movement.SetMovementEnabled(true);

            CurrentState = aggroTurret != null
                ? EnemyState.ChasingTurret
                : EnemyState.MovingToCore;
        }
    }

    // gan new target = null
    // Check aggroTurret con ton tai
    // Check crystalCore con ton tai
    // Check khi target thay doi
    private void UpdateTarget()
    {
        Transform newTarget = null;

        if (aggroTurret != null && !aggroTurret.IsDead)
        {
            newTarget = aggroTurret.transform;
        }

        else if (crystalCore != null && !crystalCore.IsDead)
        {
            aggroTurret = null;
            newTarget = crystalCore.transform;
        }

        if (currentTarget != newTarget)
        {
            currentTarget = newTarget;
            movement.SetTarget(currentTarget);
        }
    }

    // Check IsDead / attacker null / attacker IsDead
    // Lock-on turret dau tien tan cong + Debug.Log
    public void OnAttackedByTurret(TurretHealth attacker)
    {
        if (health.IsDead || attacker == null || attacker.IsDead)
            return;

        if (aggroTurret != null && !aggroTurret.IsDead)
            return;
        aggroTurret = attacker;
        Debug.Log($"{name} aggro: {attacker.name}");
    }

    public void OnDead()
    {
        CurrentState = EnemyState.Dead;
        movement.Stop();
        enabled = false;
    }


}
