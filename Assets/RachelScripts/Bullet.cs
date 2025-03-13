using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public Vector2 direction = Vector2.right;
    private float knockbackY = 0.1f; // Small fixed upward knockback

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);
        HealthManager health = other.GetComponent<HealthManager>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            float knockbackX = Mathf.Sign(player.transform.position.x - transform.position.x); // Only apply force in X direction
            Vector2 knockbackDirection = new Vector2(knockbackX, knockbackY).normalized; // Normalize to keep consistent force
            Debug.Log("Bullet knockback dir: " + knockbackDirection);
            player.Knockback(knockbackDirection);
        }
        Destroy(gameObject);
    }
}
