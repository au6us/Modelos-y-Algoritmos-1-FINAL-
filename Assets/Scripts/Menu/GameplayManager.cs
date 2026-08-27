using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para recargar niveles y volver al men�
using UnityEngine.UI; // Si usas botones de UI

public class GameplayManager : MonoBehaviour
{
    // --- Patr�n Singleton ---
    public static GameplayManager Instance { get; private set; }

    [Header("Scene Configuration")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Nombre de tu escena de men� principal

    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    // Variables de estado
    private bool isWin = false;
    private bool isGameOver = false;

    private void Awake()
    {
        // Configuraci�n del Singleton
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Nos aseguramos de que el tiempo corra al empezar
        Time.timeScale = 1f;

        currentLives = maxLives;

        // El ScreenManager ya oculta todos sus paneles registrados en su propio Awake.
    }

    // --- M�TODOS P�BLICOS DEL CONTROLADOR DE LOOP ---

    // Llam� a esto cuando el jugador muere (antes del respawn en el checkpoint).
    // Devuelve true si todav�a le quedan vidas (el respawn normal sigue su curso).
    // Devuelve false si se qued� sin vidas (ya dispar� la pantalla de Game Over).
    public bool LoseLife()
    {
        if (isGameOver || isWin) return false;

        currentLives--;
        Debug.Log($"Vidas restantes: {currentLives}");

        if (currentLives <= 0)
        {
            HandleGameOver();
            return false;
        }

        return true;
    }

    // Pantalla de Game Over compartida: la dispara LoseLife() al quedarse sin vidas,
    // y UIController la llama directo cuando se acaba el tiempo (sin importar las vidas que queden).
    public void HandleGameOver()
    {
        if (isGameOver || isWin) return; // Si ya termin� la partida, no hacemos nada

        isGameOver = true;
        Debug.Log("GAME OVER: El jugador perdi� definitivamente.");

        // 1. Pausamos el juego
        Time.timeScale = 0f;

        // 2. Mostramos el panel de Game Over
        ScreenManager.Instance.Show(ScreenId.GameOver);

        // 3. Desactivamos el control del jugador
        if (PlayerController.Instance != null) PlayerController.Instance.enabled = false;
    }

    // Llam� a esto cuando el jugador toque el trofeo del nivel
    public void HandleWin()
    {
        if (isGameOver || isWin) return; // Si ya termin� la partida, no hacemos nada

        isWin = true;
        Debug.Log("YOU WIN: �El jugador agarr� el trofeo final!");

        // 1. Pausamos el juego
        Time.timeScale = 0f;

        // 2. Mostramos el panel de Victoria
        ScreenManager.Instance.Show(ScreenId.Win);

        // 3. Desactivamos el control del jugador (opcional)
        if (PlayerController.Instance != null) PlayerController.Instance.enabled = false;
    }

    // --- M�TODOS PARA LOS BOTONES DE LA UI ---

    // Llam� a esto desde el bot�n "Reiniciar" del panel de Pausa o del de Game Over
    public void RestartLevel()
    {
        Debug.Log("Reiniciando nivel...");
        // Volvemos a poner el tiempo normal ANTES de cargar
        Time.timeScale = 1f;
        // Recargamos la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Llam� a esto desde el bot�n "Volver al Men�" de los paneles
    public void LoadMenu()
    {
        Debug.Log("Volviendo al Men� Principal...");
        // Volvemos a poner el tiempo normal ANTES de cargar
        Time.timeScale = 1f;
        // Cargamos la escena del men� principal (asegurate de tenerla en Build Settings)
        SceneManager.LoadScene(mainMenuSceneName);
    }
}