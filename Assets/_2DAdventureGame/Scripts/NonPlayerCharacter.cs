using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject dialogueBubble;

    [TextArea(2, 5)]
    public string dialogueText =
        "Допоможи полагодити всіх роботів! Натискай C, щоб стріляти.";

    void Start()
    {
        ShowBubble(false);
    }

    public void ShowBubble(bool visible)
    {
        if (dialogueBubble != null)
        {
            dialogueBubble.SetActive(visible);
        }
    }
}