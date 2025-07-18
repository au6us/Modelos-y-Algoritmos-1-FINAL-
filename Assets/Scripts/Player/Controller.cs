using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public abstract Vector3 GetMoveDir();
    public abstract bool IsJumping();
    public abstract bool IsDashing();
}
