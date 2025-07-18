using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowSecretZone : MonoBehaviour
{
    [SerializeField] Tilemap secret;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine("ShowGradial");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine("HideGradial");
        }
    }

    IEnumerator ShowGradial()
    {
        for (float f = 1; f >=0.2; f -= 0.02f)
        {
            Color color = secret.color;
            color.a = f;
            secret.color = color;   
            yield return (0.05f);
        }
    }

    IEnumerator HideGradial()
    {
        for (float f = 0.5f; f <= 1; f += 0.02f)
        {
            Color color = secret.color;
            color.a = f;
            secret.color = color;
            yield return (0.05f);
        }
    }
}
