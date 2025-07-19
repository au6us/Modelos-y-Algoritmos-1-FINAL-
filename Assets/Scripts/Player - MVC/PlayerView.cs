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
        model.OnDoubleJump += () => anim.SetBool("isDouble", true);
        model.OnLand += () => anim.SetBool("isGround", true);
        model.OnDash += () => anim.SetBool("Dash", true);
        model.OnDamage += () => anim.SetBool("Hurt", true);
    }

    private void OnDisable()
    {
        model.OnDoubleJump -= () => anim.SetBool("isDouble", true);
        model.OnLand -= () => anim.SetBool("isGround", true);
        model.OnDash -= () => anim.SetBool("Dash", true);
        model.OnDamage -= () => anim.SetBool("Hurt", true);
    }

    /// <summary>Actualiza movimiento (Speed) y dirección (flipX).</summary>
    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        sr.flipX = velocity.x < 0f;
    }

    /// <summary>Marca salto simple.</summary>
    public void SetJump(bool jumping)
    {
        anim.SetBool("Jump", jumping);
    }

    /// <summary>Marca caída.</summary>
    public void SetFall(bool falling)
    {
        anim.SetBool("Fall", falling);
    }

    /// <summary>Resetea estados al aterrizar.</summary>
    public void ResetStatesOnLand()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("isDouble", false);
        anim.SetBool("Fall", false);
        anim.SetBool("isGround", true);
    }

    /// <summary>Resetea dash después de terminar.</summary>
    public void ResetDash()
    {
        anim.SetBool("Dash", false);
    }

    /// <summary>Resetea Hurt después de animación.</summary>
    public void ResetHurt()
    {
        anim.SetBool("Hurt", false);
    }

}