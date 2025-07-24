using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask hitMask;       // capa del player u otros obstáculos

    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // Asegúrate de que el collider NO sea trigger
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
        // Si colisiona con algo de hitMask (por ejemplo el jugador)
        if (((1 << c.gameObject.layer) & hitMask) != 0)
        {
            if (c.collider.TryGetComponent<PlayerController>(out var player))
            {
                // rebote y daño usando la dirección del disparo
                // así nunca inviertes el vector
                player.Rebound(direction);
                player.TakeDamage();
            }

            DestroySelf();
            return;
        }

        // destruir contra muros o cualquier otro choque
        DestroySelf();
    }

    private void DestroySelf()
    {
        // si quisieras usar pooling, desactiva en lugar de destroy
        Destroy(gameObject);
    }

    /// <summary>
    /// Llama a este método para definir la dirección y voltear el sprite.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // FlipX si va hacia la izquierda
        sr.flipX = direction.x > 0f;
        // y aplica la velocidad
        if (rb != null)
            rb.velocity = direction * speed;
    }
}
