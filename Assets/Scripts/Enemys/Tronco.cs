using UnityEngine;

public class Tronco : EnemyBase
{
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
        bool inRange =
            Physics2D.Raycast(shootController.position, transform.right, distance, playerMask)
         || Physics2D.Raycast(shootController.position, -transform.right, distance, playerMask);

        if (inRange && Time.time > lastShoot + cooldown)
        {
            lastShoot = Time.time;
            animator.SetTrigger("Disparar");
            Instantiate(bulletPrefab, shootController.position, transform.rotation);
        }
    }
}
