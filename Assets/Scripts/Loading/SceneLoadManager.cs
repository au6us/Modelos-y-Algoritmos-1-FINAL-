using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static SceneLoadManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject loadPanel;

    [Header("Settings")]
    [SerializeField] private float fakeLoadTime = 3f; // Extra time to smooth the loading bar

    private void Awake()
    {
        // Setup Singleton and make it immortal across scenes
        if (Instance == null)
        {
            Instance = this;
            // IMPORTANTE: El objeto que tiene este script no se destruirá al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe uno (por ejemplo, si volvemos al menú), destruimos el duplicado
            Destroy(gameObject);
            return;
        }

        // Asegurarse de que el panel esté apagado al arrancar
        if (loadPanel != null) loadPanel.SetActive(false);
    }

    public void SceneLoad(int sceneIndex)
    {
        if (loadPanel != null) loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false; // Prevent auto-activation

        float progress = 0f;

        // While the real loading is not finished
        while (asyncOperation.progress < 0.9f)
        {
            progress = asyncOperation.progress / 0.9f;
            if (loadBar != null) loadBar.value = progress;
            yield return null;
        }

        // Simulación de los últimos 2 segundos (ponele) de carga
        float elapsedTime = 0f;
        while (elapsedTime < fakeLoadTime)
        {
            elapsedTime += Time.deltaTime;
            if (loadBar != null) loadBar.value = Mathf.Lerp(progress, 1f, elapsedTime / fakeLoadTime);
            yield return null;
        }

        // 1. Damos luz verde para que la escena cambie
        asyncOperation.allowSceneActivation = true;

        // 2. Esperamos a que Unity termine de hacer el swap de escenas de verdad
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // 3. RECIÉN AHORA, ya estando en el nivel nuevo, apagamos el panel de carga
        if (loadPanel != null) loadPanel.SetActive(false);
    }
}