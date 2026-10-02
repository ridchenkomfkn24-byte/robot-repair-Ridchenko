using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int healthAmount = 1; // Публічне поле для кількості здоров'я

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(healthAmount); // Передаємо healthAmount замість 1
            Destroy(gameObject);
        }
    }
}