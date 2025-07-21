using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StompDetector : MonoBehaviour
{
    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            other.attachedRigidbody.velocity.y < 0f)
        {
            other.GetComponent<PlayerController>()
                 .Rebound(Vector2.up);
            enemy.Die();
        }
    }
}
