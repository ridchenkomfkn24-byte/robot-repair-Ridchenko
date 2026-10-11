using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        Debug.Log("🟢 [PROJECTILE] Awake! Снаряд створено: " + gameObject.name, this);

        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);

        // Перевіряємо Rigidbody2D
        if (rigidbody2d == null)
        {
            Debug.LogError("❌ [PROJECTILE] НЕ ЗНАЙДЕНО Rigidbody2D!", this);
        }
        else
        {
            Debug.Log("✅ [PROJECTILE] Rigidbody2D знайдено!", this);

            Debug.Log(
                "[PROJECTILE] Gravity Scale: " + rigidbody2d.gravityScale +
                " | Body Type: " + rigidbody2d.bodyType +
                " | Simulated: " + rigidbody2d.simulated +
                " | Constraints: " + rigidbody2d.constraints,
                this
            );
        }

        // Перевіряємо SpriteRenderer
        if (spriteRenderer == null)
        {
            Debug.LogError("❌ [PROJECTILE] НЕ ЗНАЙДЕНО SpriteRenderer! Снаряд може бути невидимим!", this);
        }
        else
        {
            Debug.Log("✅ [PROJECTILE] SpriteRenderer знайдено!", this);

            if (spriteRenderer.sprite == null)
            {
                Debug.LogError("❌ [PROJECTILE] SpriteRenderer не має Sprite!", this);
            }

            if (!spriteRenderer.enabled)
            {
                Debug.LogWarning("⚠️ [PROJECTILE] SpriteRenderer вимкнений!", this);
            }

            if (spriteRenderer.color.a <= 0)
            {
                Debug.LogWarning("⚠️ [PROJECTILE] Прозорість Sprite дорівнює 0!", this);
            }

            Debug.Log(
                "[PROJECTILE] Sorting Layer: " + spriteRenderer.sortingLayerName +
                " | Order: " + spriteRenderer.sortingOrder +
                " | Колір: " + spriteRenderer.color,
                this
            );
        }
    }

    void OnEnable()
    {
        Debug.Log("🟢 [PROJECTILE] OnEnable! Об'єкт активований: " + gameObject.name, this);
    }

    void Start()
    {
        Debug.Log("🔍 [PROJECTILE] Start! Перевіряємо параметри снаряда...", this);

        Debug.Log("📍 [PROJECTILE] Позиція: " + transform.position, this);
        Debug.Log("📏 [PROJECTILE] Масштаб: " + transform.lossyScale, this);

        if (transform.lossyScale.x == 0 || transform.lossyScale.y == 0)
        {
            Debug.LogError("❌ [PROJECTILE] Масштаб снаряда дорівнює 0!", this);
        }

        // Перевіряємо камеру
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("⚠️ [PROJECTILE] Main Camera не знайдена! Перевірте тег MainCamera.", this);
        }
        else
        {
            Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);

            Debug.Log(
                "📷 [PROJECTILE] Позиція відносно камери: " + viewportPosition,
                this
            );

            if (viewportPosition.z < 0 ||
                viewportPosition.x < 0 ||
                viewportPosition.x > 1 ||
                viewportPosition.y < 0 ||
                viewportPosition.y > 1)
            {
                Debug.LogWarning(
                    "⚠️ [PROJECTILE] Точка створення снаряда знаходиться поза екраном камери!",
                    this
                );
            }
            else
            {
                Debug.Log("✅ [PROJECTILE] Точка створення знаходиться в межах камери!", this);
            }
        }
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
        Debug.Log("🚀 [PROJECTILE] Метод Launch() ВИКЛИКАНО!", this);

        Debug.Log("➡️ [PROJECTILE] Напрямок: " + direction, this);
        Debug.Log("💪 [PROJECTILE] Сила: " + force, this);
        Debug.Log("📍 [PROJECTILE] Позиція запуску: " + transform.position, this);

        if (rigidbody2d == null)
        {
            Debug.LogError("❌ [PROJECTILE] Неможливо запустити! Rigidbody2D відсутній!", this);
            return;
        }

        if (!rigidbody2d.simulated)
        {
            Debug.LogError("❌ [PROJECTILE] Rigidbody2D має Simulated = false!", this);
        }

        if (rigidbody2d.bodyType == RigidbodyType2D.Static)
        {
            Debug.LogError("❌ [PROJECTILE] Rigidbody2D має тип Static і не рухатиметься!", this);
        }

        if (direction.sqrMagnitude < 0.0001f)
        {
            Debug.LogError("❌ [PROJECTILE] Напрямок дорівнює (0, 0)! Снаряд не полетить!", this);
        }

        if (force <= 0)
        {
            Debug.LogError("❌ [PROJECTILE] Сила запуску менша або дорівнює 0!", this);
        }

        Debug.Log("⚙️ [PROJECTILE] Швидкість ДО AddForce: " + rigidbody2d.velocity, this);

        rigidbody2d.AddForce(direction * force);

        Debug.Log("✅ [PROJECTILE] AddForce виконано!", this);

        // Зелена лінія у Scene показує напрямок пострілу
        Debug.DrawRay(
            transform.position,
            (Vector3)direction.normalized * 3f,
            Color.green,
            3f
        );

        // Перевіряємо рух після фізичного оновлення
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CheckMovement());
        }
    }

    IEnumerator CheckMovement()
    {
        yield return new WaitForFixedUpdate();

        if (rigidbody2d == null)
            yield break;

        Debug.Log("🔎 [PROJECTILE] Перевірка після фізичного оновлення:", this);

        Debug.Log("📍 Позиція: " + transform.position, this);
        Debug.Log("🏃 Швидкість: " + rigidbody2d.velocity, this);

        if (rigidbody2d.velocity.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning(
                "⚠️ [PROJECTILE] Швидкість майже нульова! Перевірте Rigidbody2D, Constraints, силу та зіткнення.",
                this
            );
        }
        else
        {
            Debug.Log("✅ [PROJECTILE] Снаряд має швидкість!", this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("💥 [PROJECTILE] Увійшов у тригер: " + other.gameObject.name, this);

        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            Debug.Log("🎯 [PROJECTILE] Знайдено ворога! Викликаємо Fix()", this);
            enemy.Fix();
        }

        // Тимчасово вимкнено знищення
        // Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(
            "💥 [PROJECTILE] Зіткнення з: " + collision.gameObject.name,
            this
        );

        // Тимчасово вимкнено знищення
        // Destroy(gameObject);
    }

    void OnDisable()
    {
        Debug.LogWarning("⚠️ [PROJECTILE] Об'єкт вимкнено: " + gameObject.name, this);
    }

    void OnDestroy()
    {
        Debug.LogWarning("🗑️ [PROJECTILE] Снаряд ЗНИЩЕНО: " + gameObject.name, this);
    }
}