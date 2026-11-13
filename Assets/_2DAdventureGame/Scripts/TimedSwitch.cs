using System.Collections;
using UnityEngine;

public class TimedSwitch : MonoBehaviour
{
    public GameObject door;
    public float openTime = 4f;
    public SpriteRenderer indicator;
    public Color idleColor = new Color(0.3f, 0.9f, 0.65f);
    public Color activeColor = new Color(1f, 0.8f, 0.3f);

    private Coroutine timer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled) return;
        // Реагуємо лише на персонажа.
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }

        if (door == null)
        {
            return;
        }

        // Повторний вхід починає відлік заново.
        if (timer != null)
        {
            StopCoroutine(timer);
        }

        timer = StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        door.SetActive(false);
        if (indicator != null) indicator.color = activeColor;

        yield return new WaitForSeconds(openTime);

        door.SetActive(true);
        if (indicator != null) indicator.color = idleColor;
        timer = null;
    }

    void OnDisable()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;

            if (door != null)
            {
                door.SetActive(true);
            }
            if (indicator != null) indicator.color = idleColor;
        }
    }
}
