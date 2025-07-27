using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerModel), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Rebound Settings")]
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
    private bool isRespawning;
    private float knockbackTimer;
    private float lastFacing = 1f;
    private float originalGravity;

    // Memento
    private IMemento lastCheckpoint;


    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    private void OnEnable()
    {
        model.OnDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        model.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        if (isRespawning) return;
        StartCoroutine(DeathAndRespawnRoutine());
    }

    private IEnumerator DeathAndRespawnRoutine()
    {
        isRespawning = true;
        model.StartRespawn();

        // 1. Iniciar animación de muerte
        view.StartDeathSequence();

        // 2. Deshabilitar controles y física
        enabled = false;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        // 3. Esperar a que termine la animación de muerte
        float deathDuration = view.GetDeathAnimationLength();
        yield return new WaitForSeconds(deathDuration);

        // 4. Finalizar animación de muerte
        view.EndDeathSequence();

        // 5. Realizar respawn
        if (lastCheckpoint == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // Restaurar estado guardado
            var m = (PlayerMemento)lastCheckpoint;
            transform.position = m.Position;
            int delta = m.SavedLife - model.Life;
            if (delta > 0) model.Heal(delta);
            rb.velocity = Vector2.zero;
            isDashing = isKnockback = false;
            rb.gravityScale = originalGravity;

            // 6. Reactivar componentes
            enabled = true;
            rb.simulated = true;
            GetComponent<Collider2D>().enabled = true;
            view.ResetStatesOnLand();

            // 7. Actualizar UI de vida (SOLUCIÓN FINAL)
            GameEventManager.TriggerPlayerLifeEvent(model.Life, model.MaxLife);
        }

        model.EndRespawn();
        isRespawning = false;
    }

    public void SetCheckpoint(IMemento memento)
    {
        lastCheckpoint = memento;
    }

    private void Update()
    {
        if (isRespawning) return;

        if (isKnockback)
        {
            moveInput = Vector2.zero;
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f) isKnockback = false;
            return;
        }

        ProcessInput();
    }

    private void FixedUpdate()
    {
        if (isRespawning) return;

        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        view.SetGrounded(isGrounded);

        if (isGrounded && !wasGrounded)
        {
            model.Land();
            view.ResetStatesOnLand();
        }

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
        float h = (Input.GetKey(KeyCode.D) ? 1f : 0f)
                + (Input.GetKey(KeyCode.A) ? -1f : 0f);

        moveInput = new Vector2(h, 0f);
        if (h != 0f) lastFacing = Mathf.Sign(h);

        if (Input.GetKeyDown(KeyCode.W) && model.UseJump())
            rb.velocity = new Vector2(rb.velocity.x, model.JumpForce);

        if (Input.GetKeyDown(KeyCode.S) && model.UseDash())
            StartCoroutine(DashRoutine(lastFacing));
    }

    private IEnumerator DashRoutine(float dir)
    {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dir * model.DashSpeed, 0f);
        yield return new WaitForSeconds(model.DashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
    }


    public void Rebound(Vector2 direction)
    {
        if (isDashing)
        {
            StopAllCoroutines();
            rb.gravityScale = originalGravity;
            isDashing = false;
        }
        StartCoroutine(ApplyKnockback(direction));
    }

    private IEnumerator ApplyKnockback(Vector2 dir)
    {
        isKnockback = true;
        knockbackTimer = knockbackDuration;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir.normalized * reboundPower, ForceMode2D.Impulse);
        moveInput = Vector2.zero;
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