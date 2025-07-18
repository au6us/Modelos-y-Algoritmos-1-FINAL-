using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    public void UpdateCheckpointPosition(Vector3 newCheckpointPosition)
    {
        lastCheckpointPosition = newCheckpointPosition;
        Debug.Log("Checkpoint actualizado a: " + lastCheckpointPosition);
    }

    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
