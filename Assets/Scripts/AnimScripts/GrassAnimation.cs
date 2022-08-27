using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
    public Vector3 amount;
    public float time;

    void Start()
    {
        float randomTime = Random.Range(time-0.3f, time+0.3f);
        iTween.PunchScale(gameObject, iTween.Hash(
            "amount", amount,
            "time", randomTime,
            "looptype", iTween.LoopType.loop
        ));
    }
}
