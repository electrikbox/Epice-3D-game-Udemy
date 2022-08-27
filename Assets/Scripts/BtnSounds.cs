using UnityEngine;

public class BtnSounds : MonoBehaviour
{
    public static BtnSounds bs;
    AudioSource asound;
    public AudioClip click;


    public void Start()
    {
        asound = GetComponent<AudioSource>();
        bs = this;
    }

    public void Click() => asound.PlayOneShot(click, 0.1f);
}
