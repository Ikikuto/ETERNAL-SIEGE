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
    [SerializeField]
    private GameXP gameXP;
    [SerializeField] private EnemyHealthUI healthBar;


    private void Awake()
    {
        CurrentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
        gameXP = FindFirstObjectByType<GameXP>();
        healthBar = FindFirstObjectByType<EnemyHealthUI>();
    }

    private void Start()
    {
        CurrentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
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

        if (healthBar != null)
        {
            healthBar.SetHealth(CurrentHealth);
        }

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
        gameXP.AddXP(xpReward); // TODO: Kết nối XP Reward với XPManager

        Destroy(gameObject);
    }


}
