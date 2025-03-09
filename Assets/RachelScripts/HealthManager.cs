using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private int lives = 1;

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnLifeChanged; 
    public event Action OnDeath; 

    AudioManager audioManager;

    void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnLifeChanged?.Invoke(lives);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking DMG. Current Health: " + currentHealth + "/" + maxHealth);
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        audioManager.PlaySFXVol(audioManager.hit, 0.4f);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            LoseLife();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void LoseLife()
    {
        lives--;
        OnLifeChanged?.Invoke(lives);
        audioManager.PlaySFXVol(audioManager.death, 2.5f);

        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawning...");
        transform.position = new Vector2(0, 0); // Change this to your respawn point
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void GameOver()
    {
        Debug.Log("Player out of lives!");
        OnDeath?.Invoke();
        gameObject.SetActive(false); // hide dead player
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public int GetLives() => lives;
}
