using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerModel model;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource jumpSFX, doubleJumpSFX, hitSFX, dashSFX, footstepSFX;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem jumpParticles, doubleJumpParticles, landParticles, dashParticles;

    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        model.OnJump += () => { anim.SetBool("Jump", true); jumpSFX.Play(); jumpParticles.Play(); };
        model.OnDoubleJump += () => { anim.SetBool("isDouble", true); doubleJumpSFX.Play(); doubleJumpParticles.Play(); };
        model.OnLand += () => { anim.SetBool("isGround", true); landParticles.Play(); };
        model.OnDamage += () => { anim.SetBool("Hurt", true); hitSFX.Play(); };
        model.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        model.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        anim.SetTrigger("Die"); // Hacer la animacion de muerteeee
        Debug.Log("Player estiró la pata");
    }

    public void HandleMove(Vector2 vel)
    {
        anim.SetFloat("Speed", Mathf.Abs(vel.x));
        if (vel.x != 0f) sr.flipX = vel.x < 0f;

        bool moving = Mathf.Abs(vel.x) > 0.1f;
        bool grounded = anim.GetBool("isGround");
        if (moving && grounded && !footstepSFX.isPlaying) footstepSFX.Play();
        else if ((!moving || !grounded) && footstepSFX.isPlaying) footstepSFX.Stop();

        anim.SetBool("Hurt", false);

    }

    public void SetJump(bool j) => anim.SetBool("Jump", j);
    public void SetFall(bool f) => anim.SetBool("Fall", f);
    public void SetGrounded(bool g) => anim.SetBool("isGround", g);
    public void SetDouble(bool d) => anim.SetBool("isDouble", d);

    public void ResetStatesOnLand()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("isDouble", false);
        anim.SetBool("Fall", false);
        anim.SetBool("isGround", true);
        anim.SetBool("Hurt", false);
    }
}