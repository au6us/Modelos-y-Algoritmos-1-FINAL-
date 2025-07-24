using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerModel), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Rebound")]
    [SerializeField] private float reboundPower = 20f;
    [SerializeField] private float knockbackDuration = 0.3f;

    private PlayerModel model;
    private PlayerView view;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private bool wasGrounded;
    private bool isGrounded;
    private bool isKnockback;
    private float knockbackTimer;
    private float lastFacing = 1f;
    private float originalGravity;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (isKnockback)
        {
            // Bloquear entrada durante knockback
            moveInput = Vector2.zero;
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) isKnockback = false;
            return;
        }

        ProcessInput();
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        view.SetGrounded(isGrounded);

        if (isGrounded && !wasGrounded)
        {
            model.Land();
            view.ResetStatesOnLand();
        }

        // Solo aplicar movimiento si no está en dash o knockback
        if (!isDashing && !isKnockback)
        {
            rb.velocity = new Vector2(moveInput.x * model.MoveSpeed, rb.velocity.y);
            view.HandleMove(rb.velocity);

            bool rising = rb.velocity.y > 0.1f;
            bool falling = rb.velocity.y < -0.1f;
            view.SetJump(rising && !isGrounded);
            view.SetFall(falling && !isGrounded);

            view.SetDouble(model.JumpsLeft < model.MaxJumps - 1);
        }
    }

    private void ProcessInput()
    {
        float h = (Input.GetKey(KeyCode.D) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);
        moveInput = new Vector2(h, 0f);

        if (h != 0f) lastFacing = Mathf.Sign(h);

        if (Input.GetKeyDown(KeyCode.W) && model.UseJump())
            rb.velocity = new Vector2(rb.velocity.x, model.JumpForce);

        if (Input.GetKeyDown(KeyCode.S) && model.UseDash())
            StartCoroutine(DashRoutine(lastFacing));
    }

    private IEnumerator DashRoutine(float direction)
    {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * model.DashSpeed, 0f);
        yield return new WaitForSeconds(model.DashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    public void Rebound(Vector2 direction)
    {
        // Cancelar dash si está activo
        if (isDashing)
        {
            StopAllCoroutines();
            rb.gravityScale = originalGravity;
            isDashing = false;
        }

        // Aplicar knockback
        StartCoroutine(ApplyKnockback(direction));
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockback = true;
        knockbackTimer = knockbackDuration;

        // Resetear velocidad y aplicar fuerza
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * reboundPower, ForceMode2D.Impulse);

        // Bloquear entrada durante el knockback
        moveInput = Vector2.zero;

        // Esperar a que termine el knockback
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }

    public void TakeDamage(int damageAmount)
    {
        model.TakeDamage(damageAmount);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}