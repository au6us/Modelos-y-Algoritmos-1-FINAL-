using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask hitMask;

    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Start()
    {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (((1 << c.gameObject.layer) & hitMask) != 0)
        {
            if (c.collider.TryGetComponent<PlayerController>(out var player))
            {
                player.Rebound(direction);
                player.TakeDamage(damage);
            }

            DestroySelf();
            return;
        }

        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        sr.flipX = direction.x > 0f;
        if (rb != null) rb.velocity = direction * speed;
    }
}