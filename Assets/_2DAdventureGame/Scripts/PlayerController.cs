using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Поле для зчитування вводу з клавіатури/джойстика (Input System)
    public InputAction MoveAction;

    // Посилання на компонент Rigidbody 2D персонажа
    Rigidbody2D rigidbody2d;

    // Змінна для збереження напрямку руху
    Vector2 move;

    // --- Юніт 3: Швидкість та Здоров'я ---
    public float speed = 3.0f;
    public int maxHealth = 5;

    // Властивість лише для читання, щоб інші скрипти бачили поточне здоров'я, але не змінювали його напряму
    public int health { get { return currentHealth; } }
    int currentHealth;

    // Поля для механіки невразливості після отримання шкоди
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    void Start()
    {
        // Активуємо дію вводу
        MoveAction.Enable();

        // Отримуємо компонент Rigidbody 2D, який прикріплений до цього ж об'єкта
        rigidbody2d = GetComponent<Rigidbody2D>();

        // Встановлюємо максимальне здоров'я на старті гри
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Зчитування вводу від гравця
        move = MoveAction.ReadValue<Vector2>();

        // Закоментовано, щоб консоль не переповнювалася координатами
        // Debug.Log(move);

        // Таймер відліку невразливості
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Переміщення об'єкта з використанням публічної змінної speed
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    // Зміна здоров'я (лікування якщо amount > 0, шкода якщо amount < 0)
    public void ChangeHealth(int amount)
    {
        // Якщо отримуємо шкоду — перевіряємо невразливість
        if (amount < 0)
        {
            if (isInvincible)
            {
                return; // Гравець невразливий, шкода не завдається
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        // Обмежуємо здоров'я в діапазоні від 0 до maxHealth
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Вивід стану здоров'я в консоль (наприклад: 4/5)
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}