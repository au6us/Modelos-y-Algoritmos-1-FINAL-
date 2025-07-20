using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerModel), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour 
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private PlayerModel model;
    private PlayerView view;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private bool wasGrounded;
    private bool isGrounded;

    private float lastFacing = 1f;

    private void Awake() {
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        ProcessInput();
    }

    private void FixedUpdate() {
        // Chequeo de suelo
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        view.SetGrounded(isGrounded);

        // Al aterrizar, resetear animaciones
        if (isGrounded && !wasGrounded) {
            model.Land();
            view.ResetStatesOnLand();
        }

        // Movimiento horizontal
        if (!isDashing) {
            rb.velocity = new Vector2(moveInput.x * model.MoveSpeed, rb.velocity.y);
            view.HandleMove(rb.velocity);

            // Saltar / Caer según velocidad Y y si está en aire
            bool rising = rb.velocity.y > 0.1f;
            bool falling = rb.velocity.y < -0.1f;
            view.SetJump(rising && !isGrounded);
            view.SetFall(falling && !isGrounded);

            // Doble salto (si JumpsLeft < MaxJumps-1)
            view.SetDouble(model.JumpsLeft < model.MaxJumps - 1);
        }
    }

    private void ProcessInput() {
        // Input A/D
        float h = (Input.GetKey(KeyCode.D) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);
        moveInput = new Vector2(h, 0f);

        if (h != 0f)
        {
            lastFacing = Mathf.Sign(h);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && model.UseJump()) {
            rb.velocity = new Vector2(rb.velocity.x, model.JumpForce);
        }

        if (Input.GetKeyDown(KeyCode.S) && model.UseDash())
        {
            StartCoroutine(DashRoutine(lastFacing)); // ahora respeta hacia dónde mira
        }
    }

    private IEnumerator DashRoutine(float direction) {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * model.DashSpeed, 0f);
        yield return new WaitForSeconds(model.DashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        view.ResetDash();
    }

    private void OnDrawGizmosSelected() {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}