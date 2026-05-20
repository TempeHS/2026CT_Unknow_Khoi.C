using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float maxHealth = 1000;
    private float currentHealth;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float fillSpeed;
    [SerializeField] private Gradient colourGradient;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        Debug.Log(currentHealth);
    }

    private void UpdateHealthBar()
    {
        float targetFillAmount = currentHealth / maxHealth;
        healthBarFill.fillAmount = targetFillAmount;
        healthBarFill.color = colourGradient.Evaluate(targetFillAmount);
    }
}
