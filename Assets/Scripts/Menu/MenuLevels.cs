using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    private int starsLevelOne; // Almacena las estrellas del nivel 1
    public Button levelTwoButton; // Botón del nivel 2
    private SceneLoadManager sceneLoadManager; // Referencia al SceneLoadManager
    private JSONSaveHandler saveHandler; // Referencia al JSONSaveHandler

    private void Start()
    {
        saveHandler = FindObjectOfType<JSONSaveHandler>(); // Buscar el JSONSaveHandler
        sceneLoadManager = FindObjectOfType<SceneLoadManager>(); // Buscar el gestor de carga de escenas
        LoadStarData();

        // Desactivar el botón del Nivel 2 si no tiene suficientes estrellas del nivel 1
        if (levelTwoButton != null)
        {
            levelTwoButton.interactable = starsLevelOne >= 2; // Verifica si el jugador tiene al menos 2 estrellas
        }
    }

    private void LoadStarData()
    {
        if (saveHandler != null)
        {
            starsLevelOne = saveHandler.LoadStars(1); // Cargar las estrellas del nivel 1
        }
        else
        {
            starsLevelOne = 0; // Si no hay saveHandler, iniciar con 0 estrellas
        }
    }

    public void LevelOne()
    {
        if (sceneLoadManager != null)
        {
            sceneLoadManager.SceneLoad(1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LevelTwo()
    {
        // Verifica si el jugador tiene al menos 2 estrellas en el primer nivel
        if (starsLevelOne >= 2)
        {
            if (sceneLoadManager != null)
            {
                sceneLoadManager.SceneLoad(2); // Usa el SceneLoadManager para cargar la escena
            }
            else
            {
                SceneManager.LoadScene(2); // Carga la escena directamente si no hay SceneLoadManager
            }
        }
        else
        {
            Debug.Log("No tienes suficientes estrellas para desbloquear el Nivel 2.");
        }
    }
}
