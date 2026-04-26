using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
        public Image healthBar;
        public PlayerHealth playerHealth; 

        public void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (playerHealth != null)
            {
                healthBar.fillAmount = (float)currentHealth / maxHealth;
            }
        }
}