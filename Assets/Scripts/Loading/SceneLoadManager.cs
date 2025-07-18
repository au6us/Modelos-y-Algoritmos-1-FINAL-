using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private float fakeLoadTime = 3f; // Tiempo extra para completar la barra (ya que habia tiempito de sobra)

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false; // Evita que la escena se cargue automáticamente

        float progress = 0f;

        while (asyncOperation.progress < 0.9f) // Mientras la carga real no se complete
        {
            progress = asyncOperation.progress / 0.9f;
            loadBar.value = progress;
            yield return null;
        }

        // Simulación de los últimos 2 segundos (ponele) de carga
        float elapsedTime = 0f;
        while (elapsedTime < fakeLoadTime)
        {
            elapsedTime += Time.deltaTime;
            loadBar.value = Mathf.Lerp(progress, 1f, elapsedTime / fakeLoadTime); // Para que la barra no pegue saltitos, si no transicione más fluido
            yield return null;
        }

        // Una vez terminado el tiempo extra, se activa la escena
        asyncOperation.allowSceneActivation = true;
    }
}
