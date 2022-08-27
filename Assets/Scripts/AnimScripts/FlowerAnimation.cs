using UnityEngine;

public class FlowerAnimation : MonoBehaviour
{
    public float time;
    public float rotation;

    void Start()
    {
        float randomTime = Random.Range(time-0.5f, time+0.5f);
        iTween.RotateTo(gameObject, iTween.Hash(
            "z", rotation,
            "time", randomTime,
            "looptype", iTween.LoopType.pingPong,
            "easetype", iTween.EaseType.easeInOutSine
            ));
    }
}
