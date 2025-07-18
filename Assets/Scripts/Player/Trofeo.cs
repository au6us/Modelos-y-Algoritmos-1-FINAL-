using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trofeo : MonoBehaviour
{
    [SerializeField] private AudioSource winAudioSource;
    [SerializeField] public ParticleSystem sdpWin;
    [SerializeField] private GameplayManager gamePlayCanvas;
    [SerializeField] private Animator animatorTrophy;
    [SerializeField] private float delayWin = 4f; // Tiempo de espera antes de mostrar el panel de victoria

    private bool hasPlayedSound = false;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>(); // Buscar el LevelManager en la escena
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Player>())
            {
                animatorTrophy.SetBool("Win", true);
                sdpWin.Play();

                if (!hasPlayedSound)
                {
                    winAudioSource.Play();
                    hasPlayedSound = true; // Marcar que el sonido ya se reprodujo
                }

                // **Guardar estrellas del nivel**
                if (levelManager != null)
                {
                    levelManager.CompleteLevel();
                    Debug.Log("Nivel completado, estrellas guardadas.");
                }
                else
                {
                    Debug.LogError("No se encontró LevelManager en la escena.");
                }

                // Iniciar la corrutina para mostrar el panel de victoria después de un retraso
                StartCoroutine(ShowWinScreenAfterDelay());
            }
        }
    }

    private IEnumerator ShowWinScreenAfterDelay()
    {
        yield return new WaitForSeconds(delayWin);
        gamePlayCanvas.Onwin();
    }
}
