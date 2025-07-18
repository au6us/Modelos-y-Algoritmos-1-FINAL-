using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerModel), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerModel model;
    private PlayerView view;
    private Rigidbody2D rb;
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
    }

    private void FixedUpdate()
    {
        if (!isDashing) ApplyMovement();
    }

    private Vector2 moveInput;
    private void ProcessInput()
    {
        // Movimiento horizontal
        float h = 0f;
        if (Input.GetKey(KeyCode.A)) h = -1f;
        if (Input.GetKey(KeyCode.D)) h = +1f;
        moveInput = new Vector2(h, 0);
        if (h != 0) view.HandleMove(rb.velocity);

        // Salto con W
        if (Input.GetKeyDown(KeyCode.W) && model.UseJump())
        {
            rb.velocity = new Vector2(rb.velocity.x, model.JumpForce);
        }

        // Dash con S
        if (Input.GetKeyDown(KeyCode.S) && model.UseDash())
        {
            StartCoroutine(DashRoutine(h == 0 ? transform.localScale.x : h));
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(moveInput.x * model.MoveSpeed, rb.velocity.y);
        view.HandleMove(rb.velocity);
    }

    private IEnumerator DashRoutine(float direction)
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * model.DashSpeed, 0);
        yield return new WaitForSeconds(model.DashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y > 0.5f)
        {
            model.Land();
        }
    }
}