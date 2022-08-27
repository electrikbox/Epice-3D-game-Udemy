using UnityEngine;

public class HitEnd : MonoBehaviour
{
    public Animator bossAnimatorState;

    public void HitEndFunc() => bossAnimatorState.SetBool("bossHit", false);
}
