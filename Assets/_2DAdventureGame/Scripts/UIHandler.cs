using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    public float displayTime = 4.0f;

    private VisualElement healthBar;
    private VisualElement npcDialogue;
    private Label dialogueLabel;

    private VisualElement winScreen;
    private VisualElement loseScreen;

    private float dialogueTimer;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");

        npcDialogue = root.Q<VisualElement>("NPCDialogue");

        if (npcDialogue != null)
        {
            dialogueLabel = npcDialogue.Q<Label>("DialogueText");
            npcDialogue.style.display = DisplayStyle.None;
        }

        winScreen = root.Q<VisualElement>("WinScreenContainer");
        loseScreen = root.Q<VisualElement>("LoseScreenContainer");

        dialogueTimer = 0;
        SetHealthValue(1.0f);
    }

    void Update()
    {
        if (dialogueTimer <= 0)
        {
            return;
        }

        dialogueTimer -= Time.deltaTime;

        if (dialogueTimer <= 0 && npcDialogue != null)
        {
            npcDialogue.style.display = DisplayStyle.None;
        }
    }

    public void SetHealthValue(float percentage)
    {
        if (healthBar != null)
        {
            healthBar.style.width =
                Length.Percent(Mathf.Clamp01(percentage) * 100);
        }
    }

    public void DisplayDialogue(string message)
    {
        if (npcDialogue == null || dialogueLabel == null)
        {
            Debug.LogWarning(
                "У GameUI перевір NPCDialogue та Label DialogueText."
            );
            return;
        }

        dialogueLabel.text = message;
        npcDialogue.style.display = DisplayStyle.Flex;
        dialogueTimer = displayTime;
    }

    public void DisplayWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.style.opacity = 1;
        }
    }

    public void DisplayLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.style.opacity = 1;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}