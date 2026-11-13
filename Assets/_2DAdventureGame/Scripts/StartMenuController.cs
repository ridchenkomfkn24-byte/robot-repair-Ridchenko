using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenuController : MonoBehaviour
{
    public string levelScene = "MainScene";

    private Button playButton;
    private bool loading;

    void OnEnable()
    {
        playButton = GetComponent<UIDocument>()
            .rootVisualElement.Q<Button>("PlayButton");

        if (playButton != null)
        {
            playButton.clicked += StartGame;
        }
    }

    public void StartGame()
    {
        if (loading) return;
        loading = true;
        SceneManager.LoadScene(levelScene);
    }

    void OnDisable()
    {
        if (playButton != null)
        {
            playButton.clicked -= StartGame;
        }
    }
}
