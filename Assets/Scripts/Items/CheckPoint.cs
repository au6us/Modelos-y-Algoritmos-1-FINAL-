using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animatorCheckpoint;
    [SerializeField] private AudioSource checkpointAudioSource;

    private bool hasPlayedSound = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Animación y sonido
        animatorCheckpoint.SetBool("Checking", true);
        if (!hasPlayedSound)
        {
            checkpointAudioSource.Play();
            hasPlayedSound = true;
        }

        // 2) Guardamos el estado actual en el checkpoint
        var controller = other.GetComponent<PlayerController>();
        if (controller == null) return;

        var model = other.GetComponent<PlayerModel>();
        if (model == null) return;

        PlayerMemento memento = model.SaveState(other.transform.position);
        controller.SetCheckpoint(memento);
    }
}

