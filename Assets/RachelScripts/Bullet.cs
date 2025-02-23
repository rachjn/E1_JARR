using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public Vector2 direction = Vector2.right;

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
        Destroy(gameObject);
    }
}
