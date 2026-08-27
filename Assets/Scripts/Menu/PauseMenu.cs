using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ScreenManager.Instance.IsShowing(ScreenId.Pause)) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        ScreenManager.Instance.Show(ScreenId.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ScreenManager.Instance.Hide(ScreenId.Pause);
    }

    // �Mir� c�mo delegamos el trabajo al GameplayManager!
    public void RestartLevel()
    {
        ResumeGame(); // Sacamos la pausa
        GameplayManager.Instance.RestartLevel(); // El Manager se encarga de la escena
    }

    public void LoadMainMenu()
    {
        ResumeGame(); // Sacamos la pausa
        GameplayManager.Instance.LoadMenu(); // El Manager se encarga de la escena
    }
}