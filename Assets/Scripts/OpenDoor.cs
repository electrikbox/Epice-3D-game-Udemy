using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public static OpenDoor openDoor;
    Animator anim;
    AudioSource audioSource;
    public AudioClip doorSound;
    public BoxCollider porte, porteOpen1, porteOpen2;
    bool sound = true;

    private void Start()
    {
        openDoor = this;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.speed = 0.9f;
    }



    public void Open() => StartCoroutine("DoorDelay");



    IEnumerator DoorDelay()
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("amisFree", true);
        
        yield return new WaitForSeconds(0.57f);
        if(sound)
        {
            sound= false;
            audioSource.PlayOneShot(doorSound);
        }

        porte.enabled = false;
        porteOpen1.enabled = true;
        porteOpen2.enabled = true;
    }

}