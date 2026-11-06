using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public UIHandler uiHandler;
    public float restartDelay = 3f;

    private EnemyController[] enemies;
    private bool gameOver;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(
            FindObjectsSortMode.None
        );
    }

    void Update()
    {
        if (gameOver || player == null || uiHandler == null)
        {
            return;
        }

        if (player.health <= 0)
        {
            gameOver = true;
            uiHandler.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), restartDelay);
            return;
        }

        if (AllEnemiesFixed())
        {
            gameOver = true;
            uiHandler.DisplayWinScreen();
            Invoke(nameof(ReloadScene), restartDelay);
        }
    }

    bool AllEnemiesFixed()
    {
        if (enemies.Length == 0)
        {
            return false;
        }

        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null && enemy.isBroken)
            {
                return false;
            }
        }

        return true;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}