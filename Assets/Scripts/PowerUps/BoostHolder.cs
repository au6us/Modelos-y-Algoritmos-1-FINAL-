// BoostHolder.cs
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class BoostHolder : MonoBehaviour
{
    [HideInInspector] public float OriginalMoveSpeed;
    [HideInInspector] public int OriginalMaxJumps;
    [HideInInspector] public float OriginalDashCooldown;

    public void StoreOriginals(float ms, int mj, float dc)
    {
        OriginalMoveSpeed = ms;
        OriginalMaxJumps = mj;
        OriginalDashCooldown = dc;
    }
}
