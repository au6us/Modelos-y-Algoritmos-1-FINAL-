using UnityEngine;

public class Tronco : EnemyBase
{
    [Header("Disparo")]
    [SerializeField] private Transform shootController;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown;

    private float lastShoot;
    private Animator animator;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateBehavior()
    {
        // Voltear el sprite para mirar al jugador
        if (PlayerController.Instance != null)
        {
            bool playerIsRight = PlayerController.Instance.transform.position.x > transform.position.x;
            sr.flipX = playerIsRight;
        }

        // Comprobar rango del player
        bool inRange = Physics2D.Raycast(
            shootController.position,
            transform.right,
            distance,
            playerMask
        ) || Physics2D.Raycast(
            shootController.position,
            -transform.right,
            distance,
            playerMask
        );

        // Disparar si está en rango
        if (inRange && Time.time > lastShoot + cooldown)
        {
            lastShoot = Time.time;
            animator.SetTrigger("Disparar");
        }
    }

    public void SpawnBullet()
    {
        var go = Instantiate(bulletPrefab, shootController.position, transform.rotation);
        var bullet = go.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            // Determinar dirección al jugador
            bool playerIsRight = PlayerController.Instance.transform.position.x > transform.position.x;
            Vector2 dir = playerIsRight ? Vector2.right : Vector2.left;
            bullet.SetDirection(dir);
        }
    }
}