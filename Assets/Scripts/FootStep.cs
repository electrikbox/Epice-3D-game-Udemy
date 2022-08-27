using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioClip[] grassClips;
    public AudioClip[] woodClips;
    private AudioSource audioSource;
    private CharacterController cc;
    bool grass = true;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
    }



    public void Step()
    {
        AudioClip clip = randomeClip();
        if(cc.isGrounded)
            audioSource.PlayOneShot(clip);
    }



    private AudioClip randomeClip()
    {
        if(grass)
            return grassClips[Random.Range(0, grassClips.Length)];
        else
            return woodClips[Random.Range(0, woodClips.Length)];
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "bois")
            grass = false;
        else
            grass = true;
    }
}
