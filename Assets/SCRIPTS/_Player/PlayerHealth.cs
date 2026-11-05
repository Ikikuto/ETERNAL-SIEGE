using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();
    }

    public void TakeDamage(int damage)
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
        
    }
}
