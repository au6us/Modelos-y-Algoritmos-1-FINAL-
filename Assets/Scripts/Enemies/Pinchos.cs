using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Player jugador = collision.GetComponent<Player>();

            if (jugador != null)
            {

                jugador.TakeDamage(jugador.life, transform.position);
            }
        }
             
    }
}
