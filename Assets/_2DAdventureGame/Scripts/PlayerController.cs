using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Поле для зчитування вводу з клавіатури/джойстика (Input System)
    public InputAction MoveAction;

    // Посилання на компонент Rigidbody 2D персонажа
    Rigidbody2D rigidbody2d;

    // Змінна класу для збереження напрямку руху
    Vector2 move;

    void Start()
    {
        // Активуємо дію вводу
        MoveAction.Enable();

        // Отримуємо компонент Rigidbody 2D, який прикріплений до цього ж об'єкта
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // В Update() виконуємо ТІЛЬКИ читання вводу від гравця.
        // Це гарантує, що жодне натискання клавіші не буде пропущено.
        move = MoveAction.ReadValue<Vector2>();

        // Виведення вектора руху в консоль для перевірки
        Debug.Log(move);
    }

    void FixedUpdate()
    {
        // У FixedUpdate() виконуємо переміщення об'єкта через фізичний рушій.
        // FixedUpdate викликається синхронно з фізичним циклом (50 разів на секунду).

        // Розраховуємо нову позицію: поточні координати + напрямок * швидкість * крок часу
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;

        // Передаємо нову позицію рушію, щоб він плавно виявив колізії та зупинив робота біля стін
        rigidbody2d.MovePosition(position);
    }
}