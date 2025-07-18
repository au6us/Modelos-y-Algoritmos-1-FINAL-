using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerModel model;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        model.OnJump += HandleJump;
        model.OnDash += HandleDash;
        model.OnDamage += HandleDamage;
    }
    private void OnDisable()
    {
        model.OnJump -= HandleJump;
        model.OnDash -= HandleDash;
        model.OnDamage -= HandleDamage;
    }

    /// Llamar desde el Controller pasándole la dirección X
    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));

        if (velocity.x > 0.01f) sr.flipX = false;
        else if (velocity.x < -0.01f) sr.flipX = true;
    }

    private void HandleJump()
    {
        anim.SetTrigger("Jump");
    }

    private void HandleDash()
    {
        anim.SetTrigger("Dash");
    }

    private void HandleDamage()
    {
        anim.SetTrigger("Hurt");
    }
}