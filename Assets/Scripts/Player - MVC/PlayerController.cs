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
    private bool isGrounded;
    private bool wasGrounded;
    private bool isDashing;

    private void Awake()
    {
        model = GetComponent<PlayerModel>();
        view = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessInput();

        // Caída
        if (rb.velocity.y < -0.1f && !isGrounded && !isDashing)
        {
            view.SetFall(true);
        }
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            model.Land(); // Resetea saltos
            view.ResetStatesOnLand(); // Resetea animaciones
        }

        if (!isDashing)
        {
            ApplyMovement();
            view.HandleMove(rb.velocity);
        }
    }

    private void ProcessInput()
    {
        float h = (Input.GetKey(KeyCode.D) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);
        moveInput = new Vector2(h, 0f);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && model.UseJump())
        {
            rb.velocity = new Vector2(rb.velocity.x, model.JumpForce);
            view.SetJump(true);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.S) && model.UseDash())
        {
            float dir = h != 0f ? Mathf.Sign(h) : transform.localScale.x;
            StartCoroutine(DashRoutine(dir));
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(moveInput.x * model.MoveSpeed, rb.velocity.y);
    }

    private IEnumerator DashRoutine(float direction)
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * model.DashSpeed, 0f);
        yield return new WaitForSeconds(model.DashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        view.ResetDash(); // Cortamos bool del dash
    }

    // Visual para editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
