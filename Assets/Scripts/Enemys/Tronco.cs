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
        // 0) Voltear el sprite para mirar SIEMPRE al jugador
        if (PlayerController.Instance != null)
        {
            bool playerIsRight =
                PlayerController.Instance.transform.position.x < transform.position.x;
            // Ajusta según tu sprite: si tu sprite original mira a la derecha sin flip,
            // entonces flipX = !playerIsRight; si mira a la izquierda, usa flipX = playerIsRight;
            sr.flipX = !playerIsRight;
        }

        // 1) comprobamos rango del player
        bool inRange =
            Physics2D.Raycast(shootController.position, transform.right, distance, playerMask) ||
            Physics2D.Raycast(shootController.position, -transform.right, distance, playerMask);

        // 2) disparar si toca
        if (inRange && Time.time > lastShoot + cooldown)
        {
            lastShoot = Time.time;
            animator.SetTrigger("Disparar");
            // la bala se genera en el Animation Event con SpawnBullet()
        }
    }

    public void SpawnBullet()
    {
        var go = Instantiate(bulletPrefab, shootController.position, transform.rotation);
        var bullet = go.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            // determinamos dirección apuntando al player
            bool playerIsRight =
                PlayerController.Instance.transform.position.x > transform.position.x;
            Vector2 dir = playerIsRight ? Vector2.right : Vector2.left;
            bullet.SetDirection(dir);
        }
    }
}
