using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel, credits;
    [SerializeField] EventTrigger prototypeButton;
    [SerializeField] EventTrigger creditsButton;
    private Player player;
    void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        menuPanel.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        credits.SetActive(true);
    }

    public void GoToMain()
    {
        menuPanel.SetActive(true);
        credits.SetActive(false);
    }

    public void Shop()
    {
        SceneManager.LoadScene(3);
    }

    public void Salir()
    {
        Debug.Log("Se cerrará el juego");
        Application.Quit();

    }
}
