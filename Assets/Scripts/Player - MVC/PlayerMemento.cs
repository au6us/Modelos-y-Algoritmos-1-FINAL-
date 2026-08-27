using UnityEngine;

public interface IMemento
{
    // Interfaz "angosta": lo único que el Caretaker (PlayerController) puede leer directamente.
    // El resto del estado (vida) queda oculto y solo lo lee el propio Originator (PlayerModel).
    Vector3 Position { get; }
}

public class PlayerMemento : IMemento
{
    public Vector3 Position { get; }
    public int SavedLife { get; }

    public PlayerMemento(Vector3 pos, int life)
    {
        Position = pos;
        SavedLife = life;
    }
}