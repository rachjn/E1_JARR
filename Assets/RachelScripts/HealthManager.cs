using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private int lives = 1;

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnLifeChanged; 
    public event Action OnDeath; 
    
    [SerializeField] private HealthBarUI HealthUI;
    [SerializeField] private GameOverUI gameOverUI;

    AudioManager audioManager;

    void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnLifeChanged?.Invoke(lives);
        HealthUI.UpdateHealthBar(1, lives);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking DMG. Current Health: " + currentHealth + "/" + maxHealth);
        HealthUI.UpdateHealthBar((float)(currentHealth-damage) / maxHealth, lives);
        if (currentHealth <= 0) {
            return;
        }

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
            HealthUI.UpdateHealthBar(1, lives);
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
        HealthUI.UpdateHealthBar(0, lives);
        Debug.Log("Player out of lives!");
        OnDeath?.Invoke();
        
        // GameOverUI gameOverUI = FindObjectOfType<GameOverUI>();
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOverScreen();
        }

        gameObject.SetActive(false); // Hide the player
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            Debug.Log("Player hit deadly object!");

            // Set health to zero and immediately lose a life
            currentHealth = 0;
            LoseLife();
        }
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public int GetLives() => lives;
}
