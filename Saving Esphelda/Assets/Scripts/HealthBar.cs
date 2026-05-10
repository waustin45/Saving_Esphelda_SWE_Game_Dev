using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
        public Image healthBar;

        public void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (playerHealth != null)
            {
                healthBar.fillAmount = (float)currentHealth / maxHealth;
            }
        }
}