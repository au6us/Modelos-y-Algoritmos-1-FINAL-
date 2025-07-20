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
    }

    private void OnDisable()
    {
    }

    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));

        // Actualiza en donde mira el player, antes se quedaba mirando siempre a la derecha cuando se dejaba de caminar
        if (velocity.x != 0)
        {
            sr.flipX = velocity.x < 0f;
        }

        // Para los sonidos de pasos 
        bool isMoving = Mathf.Abs(velocity.x) > 0.1f;
        bool isGrounded = anim.GetBool("isGround");

        if (isMoving && isGrounded)
        {
            if (!footstepSFX.isPlaying)
            {
                footstepSFX.Play();
            }
        }
        else if (footstepSFX.isPlaying)
        {
            footstepSFX.Stop();
        }
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
