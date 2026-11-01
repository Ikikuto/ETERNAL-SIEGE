using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private int xpReward = 5;

    private EnemyAI enemyAI;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
    public int XpReward => xpReward;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
    }


    // Check enemy IsDead or damage <= 0
    // Update CurrentHealth + Debug.Log
    // Check CurrentHealth
    // Check enemy attacked by turret
    public void TakeDamage(float damage, TurretHealth attacker = null)
    {
        if (IsDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(
            0f,
            CurrentHealth - damage
        );

        Debug.Log($"{name} HP: {CurrentHealth}");

        if (CurrentHealth <= 0f)
        {
            Die();
            return;
        }

        // Zombie bị Turret tấn công thì chuyển aggro
        if (attacker != null)
        {
            enemyAI.OnAttackedByTurret(attacker);
        }
    }


    private void Die()
    {
        IsDead = true;

        enemyAI.OnDead();

        // TODO: Kết nối XP Reward với XPManager
        Destroy(gameObject);
    }


}
