using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerModel model;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource doubleJumpSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private AudioSource footstepSFX;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem doubleJumpParticles;
    [SerializeField] private ParticleSystem landParticles;
    [SerializeField] private ParticleSystem dashParticles;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    private System.Action onJumpHandler;
    private System.Action onDoubleJumpHandler;
    private System.Action onLandHandler;
    private System.Action onDamageHandler;
    private System.Action onDashHandler;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        model.OnJump += () => {anim.SetBool("Jump", true);
            jumpSFX.Play();
            jumpParticles.Play();
        };

        model.OnDoubleJump += () => {anim.SetBool("isDouble", true);
            doubleJumpSFX.Play();
            doubleJumpParticles.Play();
        };

        model.OnLand += () => {ResetStatesOnLand();
            landParticles.Play();
        };

        model.OnDamage += () => {SetHurt(true);
            hitSFX.Play();

        };

        model.OnDash += () => {
            dashSFX.Play();
            dashParticles.Play();
        };


        model.OnJump += onJumpHandler;
        model.OnDoubleJump += onDoubleJumpHandler;
        model.OnLand += onLandHandler;
        model.OnDamage += onDamageHandler;
        model.OnDash += onDashHandler;
    }

    private void OnDisable()
    {
        model.OnJump -= onJumpHandler;
        model.OnDoubleJump -= onDoubleJumpHandler;
        model.OnLand -= onLandHandler;
        model.OnDamage -= onDamageHandler;
        model.OnDash -= onDashHandler;
    }

    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        sr.flipX = velocity.x < 0f; 
    }

    public void SetJump(bool jumping)
    {
        anim.SetBool("Jump", jumping);
    }

    public void SetFall(bool falling)
    {
        anim.SetBool("Fall", falling);
    }

    public void SetGrounded(bool grounded)
    {
        anim.SetBool("isGround", grounded);
    }

    public void SetDouble(bool isDouble)
    {
        anim.SetBool("isDouble", isDouble);
    }

    public void SetHurt(bool hurt)
    {
        anim.SetBool("Hurt", hurt);
    }

    public void ResetStatesOnLand()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("isDouble", false);
        anim.SetBool("Fall", false);
        anim.SetBool("isGround", true);
        anim.SetBool("Dash", false);
        anim.SetBool("Hurt", false);
    }

    public void ResetDash()
    {
        anim.SetBool("Dash", false);
    }

    public void ResetHurt()
    {
        anim.SetBool("Hurt", false);
    }
}
