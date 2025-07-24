using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask hitMask;      // capar jugador + suelos, muros, etc.
    [SerializeField] private Vector2 direction = Vector2.right;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Asegurate que el collider NO es trigger
    }

    private void OnEnable()
    {
        // Destruir si no golpea nada tras un tiempo
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Start()
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        // ¿fue el Player?
        if (((1 << c.gameObject.layer) & hitMask) != 0)
        {
            if (c.collider.TryGetComponent<PlayerController>(out var player))
            {
                // knockback sencillo hacia atrás
                Vector2 normal = c.GetContact(0).normal;
                player.Rebound(normal);
                player.TakeDamage();
            }
            DestroySelf();
            return;
        }

        // opcional: destruir contra muros/objetos
        DestroySelf();
    }

    private void DestroySelf()
    {
        gameObject.SetActive(false);
        // o bien Destroy(gameObject);
    }

    /// <summary>
    /// Llamar desde el Tronco justo después de Instantiate:
    /// bullet.SetDirection(transform.right);
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        rb.velocity = direction.normalized * speed;
    }
}
