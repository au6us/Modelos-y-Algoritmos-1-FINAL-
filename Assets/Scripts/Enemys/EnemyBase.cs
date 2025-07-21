using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyBase Prefab { get; set; }
    public event Action<EnemyBase> OnDie;

    [SerializeField] protected int pointValue = 100;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float deathAnimDuration = 0.5f;

    protected Animator anim;
    protected Collider2D col;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!isDead)
            UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    private void OnCollisionEnter2D(Collision2D c)
    {
        // Solo knockback lateral, sin matar al enemigo
        if (((1 << c.gameObject.layer) & playerLayer) != 0 &&
            Mathf.Abs(c.relativeVelocity.x) > 0.5f)
        {
            var player = c.collider.GetComponent<PlayerController>();
            if (player != null)
                player.Rebound(c.GetContact(0).normal);
        }
    }

    /// <summary>
    /// Llamado por StompDetector al pisar al enemigo.
    /// </summary>
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        col.enabled = false;            // quita colisión
        anim.SetBool("Walk", false);
        anim.SetBool("Die", true);      // muestra animación de muerte

        OnDie?.Invoke(this);
        GameEventManager.EnemyKilled(pointValue);

        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(deathAnimDuration);
        // resetear estado
        isDead = false;
        anim.SetBool("Die", false);
        // dejar limpio para patrullar de nuevo
        ResetAfterDeath();
        PoolManager.Instance.Release(this);
    }

    /// <summary>
    /// Aquí resetea cualquier estado específico antes de volver al pool.
    /// </summary>
    protected virtual void ResetAfterDeath()
    {
        col.enabled = true;
    }
}
