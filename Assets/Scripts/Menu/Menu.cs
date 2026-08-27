using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] EventTrigger prototypeButton;
    [SerializeField] EventTrigger creditsButton;
    private PlayerModel playerModel;
    void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        ScreenManager.Instance.Show(ScreenId.MainMenu);
    }

    public void ShowCredits()
    {
        ScreenManager.Instance.Show(ScreenId.Credits);
    }

    public void GoToMain()
    {
        ShowMain();
    }

    public void Shop()
    {
        SceneManager.LoadScene(3);
    }

    public void Salir()
    {
        Debug.Log("Se cerrar� el juego");
        Application.Quit();

    }
}
