using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Debug.Log("Снаряд створено в пам'яті!");
    }

    void Update()
    {
        // Тимчасово вимкнено видалення за відстанню
        /*
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
        */
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        Debug.Log("Снаряд запущено з силою: " + force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Снаряд пролетів крізь тригер: " + other.gameObject.name);

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
        }

        // Тимчасово вимкнено знищення при влучанні
        // Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Снаряд зіткнувся з колайдером: " + collision.gameObject.name);

        // Тимчасово вимкнено знищення при зіткненні
        // Destroy(gameObject);
    }
}