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

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void UpdateBehavior()
    {
        // 1) comprueba si el jugador está en rango a la izquierda o derecha
        bool inRange =
            Physics2D.Raycast(shootController.position, transform.right, distance, playerMask) ||
            Physics2D.Raycast(shootController.position, -transform.right, distance, playerMask);

        // 2) si está en rango y ya pasó el cooldown → dispara
        if (inRange && Time.time > lastShoot + cooldown)
        {
            lastShoot = Time.time;
            animator.SetTrigger("Disparar");
            // **NO** instancies la bala aquí; la instanciamos en SpawnBullet()  
            //  para que coincida con el fotograma exacto de tu animación
        }
    }

    /// <summary>
    /// Este método se llamará desde un Animation Event
    /// justo en el fotograma en que quieras que salga la bala.
    /// </summary>
    public void SpawnBullet()
    {
        // 3) Instanciamos la bala y configuramos su dirección
        var go = Instantiate(bulletPrefab, shootController.position, transform.rotation);
        var bullet = go.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            // Decide si la bala sale a la derecha o a la izquierda
            Vector2 dir = (PlayerController.Instance.transform.position.x > transform.position.x)
                          ? Vector2.right
                          : Vector2.left;
            bullet.SetDirection(dir);
            // bullet.hitMask = 1 << LayerMask.NameToLayer("Player"); // opcional
        }
    }
}
