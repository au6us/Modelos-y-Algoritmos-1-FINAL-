using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerModel model;
    private Animator anim;

    private void Awake()
    {
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

    public void HandleMove(Vector2 velocity)
    {
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
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