using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StompDetector : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private float reboundForce = 10f;
    [SerializeField] private LayerMask playerLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) == 0)
            return;

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Rebound(Vector2.up * reboundForce); // rebota hacia arriba
            enemy.Die(); // mata al enemigo
        }
    }
}
