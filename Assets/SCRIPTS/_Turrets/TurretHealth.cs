
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    private TurretCombat turretCombat;

    //Start: get component TurretCombat
    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;

        turretCombat = GetComponent<TurretCombat>();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(
            0f,
            CurrentHealth - damage
        );

        Debug.Log(
            $"{name} HP: {CurrentHealth}/{maxHealth}"
        );

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    // check IsDead
    // ...
    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        if (turretCombat != null)
        {
            turretCombat.enabled = false;
        }

        Debug.Log($"{name} destroyed!");

        Destroy(gameObject);
    }
}
