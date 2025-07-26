using UnityEngine;

// 1) Interfaz marca-memento
public interface IMemento { }

// 2) Implementación concreta para el Player
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
