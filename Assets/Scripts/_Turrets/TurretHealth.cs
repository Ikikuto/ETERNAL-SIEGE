using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private float maxHealth = 100f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private TurretCombat turretCombat;

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

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        Debug.Log($"{name} HP: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0f)
            Die();
    }

    // Hồi 20% Max HP: gọi HealPercent(0.2f).
    public void HealPercent(float percent)
    {
        if (IsDead || percent <= 0f)
            return;

        float healAmount = maxHealth * percent;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + healAmount);

        Debug.Log($"{name} healed {healAmount} HP. HP: {CurrentHealth}/{maxHealth}");
        // Nếu bổ sung thanh máu trụ về sau, cập nhật thanh máu tại đây.
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        if (turretCombat != null)
            turretCombat.enabled = false;

        Debug.Log($"{name} destroyed!");
        Destroy(gameObject);
    }
}
