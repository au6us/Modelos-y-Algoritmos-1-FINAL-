using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerModel model;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource jumpSFX, doubleJumpSFX, hitSFX, dashSFX, footstepSFX, deathSFX;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem jumpParticles, doubleJumpParticles, landParticles, dashParticles, deathParticles;

    private SpriteRenderer sr;
    private Animator anim;
    private bool isDying = false;

    // --- NUEVO: Variable para guardar el proceso de parpadeo ---
    private Coroutine blinkCoroutine;

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
        model.OnDash += () => { dashSFX.Play(); dashParticles.Play(); };
        model.OnDeath += StartDeathSequence;

        // Suscripción al evento de invencibilidad (Escudo roto)
        model.OnInvincibilityChanged += HandleInvincibility;
    }

    private void OnDisable()
    {
        model.OnDeath -= StartDeathSequence;
        model.OnInvincibilityChanged -= HandleInvincibility;
    }

    // --- CORRECCIÓN AQUÍ ---
    private void HandleInvincibility(bool isInvincible)
    {
        if (isInvincible)
        {
            // Si ya estaba parpadeando por alguna razón, lo reiniciamos
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

            // Iniciamos y GUARDAMOS la referencia
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
        else
        {
            // Detenemos ESPECÍFICAMENTE la que guardamos
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            // Aseguramos que el personaje quede visible (Opaco)
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            Color c = sr.color;
            c.a = 0.5f; // Semi-transparente
            sr.color = c;
            yield return new WaitForSeconds(0.1f);

            c.a = 1f; // Opaco
            sr.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // ... (El resto de tus métodos siguen igual) ...
    public void StartDeathSequence()
    {
        if (isDying) return;
        isDying = true;
        anim.SetBool("Hurt", false);
        anim.SetBool("Jump", false);
        anim.SetBool("isDouble", false);
        anim.SetBool("Fall", false);
        anim.SetBool("PlayerDeath", true);
    }

    public void EndDeathSequence()
    {
        anim.SetBool("PlayerDeath", false);
        isDying = false;
    }

    public float GetDeathAnimationLength()
    {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == "PlayerDeath") return clip.length;
        }
        return 1f;
    }

    public void HandleMove(Vector2 vel)
    {
        if (isDying) return;
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
        anim.SetBool("PlayerDeath", false);
    }
}