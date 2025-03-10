using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForegoundImage;
    [SerializeField] private TMP_Text healthText;
    // [SerializeField] private HealthManager HealthManager;
    public void UpdateHealthBar(float currentHealth, int lives)
    {
        Debug.Log("bar should move:" + currentHealth);
        _healthBarForegoundImage.fillAmount = currentHealth;
        healthText.text = "Lives: " + lives.ToString();

    }
}
