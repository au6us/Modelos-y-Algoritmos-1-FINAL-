using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Animator animatorCheckpoint; 
    [SerializeField] private AudioSource checkpointAudioSource; 
    private bool hasPlayedSound = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.CompareTag("Player"))
        {
            CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
            if (checkpointManager != null)
            {
                checkpointManager.UpdateCheckpointPosition(transform.position);
                Debug.Log("Jugador ha activado el checkpoint en: " + transform.position);
            }
            
            animatorCheckpoint.SetBool("Checking", true);

            // Reproducir el efecto de sonido solo una vez
            if (!hasPlayedSound)
            {
                checkpointAudioSource.Play();
                hasPlayedSound = true; // Marcar que el sonido ya se reprodujo
            }            
        }
    }
}