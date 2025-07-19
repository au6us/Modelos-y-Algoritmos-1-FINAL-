using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    [Header("References")]
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
        model.OnJump += () => anim.SetBool("Jump", true);
        model.OnDoubleJump += () => anim.SetBool("isDouble", true);
        model.OnLand += ResetStatesOnLand;
        model.OnDamage += () => SetHurt(true);
    }

    private void OnDisable()
    {
        model.OnJump -= () => anim.SetBool("Jump", true);
        model.OnDoubleJump -= () => anim.SetBool("isDouble", true);
        model.OnLand -= ResetStatesOnLand;
        model.OnDamage -= () => SetHurt(true);
    }

    /// <summary>Actualiza Speed y voltea el sprite.</summary>
    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        sr.flipX = velocity.x < 0f;
    }

    /// <summary>Marca salto (subida).</summary>
    public void SetJump(bool jumping)
    {
        anim.SetBool("Jump", jumping);
    }

    /// <summary>Marca caída.</summary>
    public void SetFall(bool falling)
    {
        anim.SetBool("Fall", falling);
    }

    /// <summary>Marca estado de suelo (enSuelo).</summary>
    public void SetGrounded(bool grounded)
    {
        anim.SetBool("isGround", grounded);
    }

    /// <summary>Marca segundo salto.</summary>
    public void SetDouble(bool isDouble)
    {
        anim.SetBool("isDouble", isDouble);
    }

    /// <summary>Marca daño (Hurt).</summary>
    public void SetHurt(bool hurt)
    {
        anim.SetBool("Hurt", hurt);
    }

    /// <summary>Resetea estados al aterrizar (Jump, Double, Fall).</summary>
    public void ResetStatesOnLand()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("isDouble", false);
        anim.SetBool("Fall", false);
        anim.SetBool("isGround", true);
        anim.SetBool("Dash", false);
        anim.SetBool("Hurt", false);
    }

    /// <summary>Resetea dash tras la rutina.</summary>
    public void ResetDash()
    {
        anim.SetBool("Dash", false);
    }

    /// <summary>Resetea Hurt tras la animación.</summary>
    public void ResetHurt()
    {
        anim.SetBool("Hurt", false);
    }
}
