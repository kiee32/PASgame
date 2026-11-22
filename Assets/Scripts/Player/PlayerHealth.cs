using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    // (nyawa sekarang, nyawa maksimal)
    public event Action<int, int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void KenaDamage(int jumlah)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - jumlah);
        Debug.Log("Player kena damage " + jumlah + ", nyawa sisa: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) Mati();
    }

    void Mati()
    {
        Debug.Log("Player mati!");

        PlayerMovement gerak = GetComponent<PlayerMovement>();
        if (gerak != null) gerak.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}