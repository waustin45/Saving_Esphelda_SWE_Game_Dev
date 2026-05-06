using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public int maxHealth = 10;
    public int currentHealth;

    PlayerDeath PlayerDeath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        PlayerDeath = GetComponent<PlayerDeath>();
        if (PlayerDeath == null)
        {
            Debug.LogWarning("PlayerHealth: PlayerDeath component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth); 
        }
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            if (PlayerDeath != null)
            {
                PlayerDeath.KillPlayer();
            }
            else
            {
                Debug.LogError("PlayerHealth: PlayerDeath component is null. Cannot kill player.");
            }
        } 
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //makes sure health doesn't exceed maximum health
        
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}