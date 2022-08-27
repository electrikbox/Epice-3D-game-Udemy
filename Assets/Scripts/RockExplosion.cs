using System.Collections;
using UnityEngine;

public class RockExplosion : MonoBehaviour
{
    public GameObject explosion;
    public AudioSource aSource;
    public AudioClip explosionSound;

    bool canInstantiate = true;


    private void FixedUpdate()
    {
        if(PlayerInfos.pi.gotKey && canInstantiate)
        {
            canInstantiate = false;
            StartCoroutine(Explosion());
        }
    }



    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1.5f);
        aSource.PlayOneShot(explosionSound, 4f);
        GameObject go = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * 2;
        Destroy(gameObject);
        Destroy(go, 2);
        canInstantiate = true;
    }
}
