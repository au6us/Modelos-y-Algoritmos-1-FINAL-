using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItem : MonoBehaviour
{
    public Animator animatorStart;
    private GameObject player;
    private Rigidbody2D playerRb;

    [SerializeField] private AudioSource yeyAudioSource;
    [SerializeField] private ParticleSystem particulasConfeti;

    private bool hasPlayed = false;
    private bool hasPlayedSound = false;



    private void Start()
    {        
        player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {        
        if (Mathf.Abs(playerRb.velocity.x) > 0.1f) 
        {          
            animatorStart.SetBool("Moving", true);

            if (!hasPlayed)
            {
                particulasConfeti.Play();
                hasPlayed = true; // Marcar que las partículas ya se activaron
            }

            if (!hasPlayedSound)
            {
                yeyAudioSource.Play();
                hasPlayedSound = true; // Marcar que el sonido ya se reprodujo
            }
        }
        else
        {
            // Desactivar la animación si el jugador no se mueve
            animatorStart.SetBool("Moving", false);
        }

    }
}