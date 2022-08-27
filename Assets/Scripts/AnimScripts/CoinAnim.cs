using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnim : MonoBehaviour
{
    public Vector3 dir;

    void FixedUpdate() => transform.Rotate(dir * Time.fixedDeltaTime);
}
