using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of bullet
    public int damage = 10;
    public Vector2 direction = Vector2.right; // Default direction

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // Move bullet
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);
        Destroy(gameObject); // Destroy bullet on collision
    }
}
