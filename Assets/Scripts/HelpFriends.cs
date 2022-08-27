using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HelpFriends : MonoBehaviour
{
    GameObject cage;

    public GameObject mobDestroyFX, decolleFX;
    private GameObject bump;
    public Text infoText;
    public AudioClip explosion, launch;

    bool canOpen = false;
        
    private AudioSource audioSource;



    private void Start() => audioSource = GetComponent<AudioSource>();



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "cage")
        {
            cage = other.gameObject;
            canOpen = true;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "cage")
        {
            cage = null;
            infoText.text = "";
            canOpen = false;
        }
    }



    public void SaveFriends()
    {
        canOpen = false;

        PauseScript.amisRestants --;

        SphereCollider zone = cage.GetComponent<SphereCollider>();
        zone.radius = 0.2f;
            
        GameObject go = Instantiate(mobDestroyFX, cage.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(explosion, 0.5f);
            
        Destroy(cage.GetComponent<MeshRenderer>(), 0.1f);
        Destroy(cage.GetComponent<BoxCollider>(), 0.1f);
        Destroy(go, 2.5f);
            
        StartCoroutine("freeFriend");
        infoText.text = "";
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "bump" && canOpen && cage != null)
        {
            bump = hit.gameObject;
            SaveFriends();
            StartCoroutine(BumpDly());
        }
    }


    IEnumerator freeFriend()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerInfos.pi.SetPoussinBar();
        PlayerInfos.pi.nbPoussin ++;

        Canvas bulle = cage.GetComponentInChildren<Canvas>();
        bulle.enabled = true;
        iTween.RotateAdd(bulle.gameObject, new Vector3(0, 0, 720), 1f);
        iTween.ScaleFrom(bulle.gameObject, Vector3.zero, 0.6f);
        yield return new WaitForSeconds(0.7f);

        iTween.RotateAdd(bulle.gameObject, new Vector3(0, 0, 360), 1f);
        iTween.ScaleTo(bulle.gameObject, Vector3.zero, 1f);

        GameObject go = Instantiate(decolleFX, cage.transform.position, Quaternion.identity);
        iTween.MoveAdd(cage, Vector3.forward * 10, 0.7f);
        audioSource.PlayOneShot(launch, 0.5f);

        yield return new WaitForSeconds(0.2f);

        Destroy(cage, 1f);
        Destroy(go, 2f);
    }



    IEnumerator BumpDly()
    {
        iTween.PunchScale(bump, new Vector3(1.1f, 0f, 1.1f), 0.9f);
        yield return new WaitForSeconds(0.7f);
        iTween.ScaleTo(bump, Vector3.zero, 0.2f);
        Destroy(bump, 0.5f);
    }
}