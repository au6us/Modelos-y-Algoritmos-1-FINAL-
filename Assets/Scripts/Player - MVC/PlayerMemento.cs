using UnityEngine;

public interface IMemento { }

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