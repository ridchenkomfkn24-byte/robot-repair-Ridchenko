using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction LaunchAction;
    public InputAction TalkAction;

    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;
    public AudioClip hitSound;
    public AudioClip healSound;
    public AudioClip launchSound;

    public int health
    {
        get { return currentHealth; }
    }

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private AudioSource audioSource;

    private Vector2 move;
    private Vector2 moveDirection = Vector2.right;

    private int currentHealth;
    private bool isInvincible;
    private float damageCooldown;

    private NonPlayerCharacter lastNonPlayerCharacter;

    void Start()
    {
        MoveAction.Enable();
        LaunchAction.Enable();
        TalkAction.Enable();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f)
            || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection = move.normalized;
        }

        if (animator != null)
        {
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0)
            {
                isInvincible = false;
            }
        }

        if (LaunchAction.WasPressedThisFrame())
        {
            Launch();
        }

        CheckForNPC();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position
            + move * speed * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(position);
    }

    void CheckForNPC()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            rigidbody2d.position + Vector2.up * 0.2f,
            moveDirection,
            1.5f,
            LayerMask.GetMask("NPC")
        );

        NonPlayerCharacter npc = null;

        if (hit.collider != null)
        {
            npc = hit.collider.GetComponent<NonPlayerCharacter>();
        }

        // Ховаємо підказку, якщо відвернулися або дивимося на іншого NPC.
        if (lastNonPlayerCharacter != npc)
        {
            if (lastNonPlayerCharacter != null)
            {
                lastNonPlayerCharacter.ShowBubble(false);
            }

            lastNonPlayerCharacter = npc;
        }

        if (npc == null)
        {
            return;
        }

        npc.ShowBubble(true);

        if (TalkAction.WasPressedThisFrame()
            && UIHandler.instance != null)
        {
            UIHandler.instance.DisplayDialogue(npc.dialogueText);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (Mathf.Clamp(currentHealth + amount, 0, maxHealth) == currentHealth)
        {
            return;
        }

        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            damageCooldown = timeInvincible;

            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }

        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );
        PlaySound(amount < 0 ? hitSound : healSound);

        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetHealthValue(
                currentHealth / (float)maxHealth
            );
        }
    }

    void Launch()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f,
            Quaternion.identity
        );

        Projectile projectile =
            projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Launch(moveDirection, 300);
            PlaySound(launchSound);
        }

        if (animator != null)
        {
            animator.SetTrigger("Launch");
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void OnDisable()
    {
        MoveAction.Disable();
        LaunchAction.Disable();
        TalkAction.Disable();

        if (lastNonPlayerCharacter != null)
        {
            lastNonPlayerCharacter.ShowBubble(false);
        }
    }
}
