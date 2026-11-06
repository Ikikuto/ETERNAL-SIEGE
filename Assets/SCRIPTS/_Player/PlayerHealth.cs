using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private PlayerHealthUI playerHealthUI; 

    private float currentHealth;

    public float CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void Heal(float amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || currentHealth <= 0) return;
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateUI();

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
        gameObject.SetActive(false);
    }

private void UpdateUI()
    {
        if (playerHealthUI != null)
        {
            playerHealthUI.SetHealth(currentHealth);
        }
    }
}
