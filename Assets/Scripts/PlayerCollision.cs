using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCollision : MonoBehaviour
{
    public static PlayerCollision pc;

    public GameObject pickupFX, CFX2_Big_Splash, loot, key, explosion, explosionBoss, deathBoss, keyMagic;
    public AudioClip coinSound, mobDeadSound, hurt, water, explosionSound, bossExplosionSound;
    public SkinnedMeshRenderer rendPlayer, bossRend;
    public Animator bossAnimatorState;
    public NavMeshAgent bossAgent;
    public Collider BossHurtCol, bossColHit;

    private bool canInstantiate = true;
    // public int bossHit = 0;
    public int bossLives = 3;
    public bool isInvincible = false;

    private AudioSource audioSource, music;
    GameObject goKeyMagic;


    private void Start()
    {
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        pc = this;
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "coin":
                audioSource.PlayOneShot(coinSound, 0.2f);
                GameObject goCoin = Instantiate(pickupFX, other.transform.position, Quaternion.identity);
                Destroy(goCoin, 1f);
                PlayerInfos.pi.GetCoin();
                Destroy(other.gameObject);
                break;

            case "water":
                audioSource.PlayOneShot(water, 0.5f);
                GameObject goWater = Instantiate(CFX2_Big_Splash, gameObject.transform.position, Quaternion.identity);
                Destroy(goWater, 1f);
                break;

            case "zoom":
                if(bossLives != 0)
                    bossPosition.bp.ShowBossHearts();
                break;

            case "fin":
                PauseScript.pauseScript.LoadWinScene();
                break;

            case "key":
                audioSource.PlayOneShot(coinSound, 0.2f);
                GameObject goKey = Instantiate(pickupFX, other.transform.position, Quaternion.identity);
                Destroy(goKey, 1f);
                PlayerInfos.pi.GetKey();
                Destroy(other.gameObject);
                Destroy(goKeyMagic);
                break;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "hurt" && canInstantiate && !isInvincible)
            PlayerGetDamaged(-1, other.transform.gameObject, 3.5f);

        if(other.gameObject.tag == "boss" && canInstantiate && !isInvincible)
            PlayerGetDamaged(-2, other.transform.gameObject, 5.5f);
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "zoom" && bossLives != 0)
        {
            bossPosition.bp.HideBossHearts();
        }
    }


    
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if(other.gameObject.tag == "mob" && canInstantiate)
        {
            canInstantiate = false;
            iTween.PunchScale(other.gameObject.transform.parent.gameObject, new Vector3(1.1f,1.1f,1f), 1.0f);
            Instantiate(loot, transform.position + transform.forward, Quaternion.identity * Quaternion.Euler(90,0,0));
            audioSource.PlayOneShot(mobDeadSound, 0.7f);
            Destroy(other.gameObject.transform.parent.gameObject.transform.parent.gameObject, 0.2f);
            StartCoroutine(ResetInstantiate(0.2f));
        }

        if(other.gameObject.tag == "bossKill" && canInstantiate)
        {
            canInstantiate = false;
            iTween.PunchScale(other.gameObject.transform.parent.gameObject, new Vector3(1.1f,1.1f,1.1f), 0.5f);
            
            bossLives --;
            bossPosition.bp.SetBossHealthBar();
            
            StartCoroutine(BossHit(other));
            StartCoroutine(ResetInstantiate(0.2f));
        }
    }



    public void PlayerGetDamaged(int health, GameObject go, float multi)
    {
        PlayerInfos.pi.SetHeatlth(health);
        isInvincible = true;
        canInstantiate = false;
        if(PlayerInfos.pi.playerHealth != 0)
            iTween.MoveAdd(gameObject, Vector3.back * multi, 1f);
        audioSource.PlayOneShot(hurt, 0.7f);
        StartCoroutine(ResetInstantiate(0.6f));
        StartCoroutine("ResetInvincible");
    }



    private void ColliderOnOff()
    {
        BossHurtCol.enabled = !BossHurtCol.enabled;
        bossColHit.enabled = !bossColHit.enabled;
    }



    IEnumerator ResetInstantiate(float time)
    {
        yield return new WaitForSeconds(time);
        canInstantiate = true;
    }



    IEnumerator ResetInvincible()
    {
        if(PlayerInfos.pi.playerHealth != 0)
        {
            for(int i=0 ; i < 10 ; i++)
            {
                yield return new WaitForSeconds(0.1f);
                rendPlayer.enabled = !rendPlayer.enabled;
            }
        yield return new WaitForSeconds(0.1f);
        rendPlayer.enabled = true;
        }

        isInvincible = false;
    }



    IEnumerator BossHit(ControllerColliderHit otherObject)
    {
        if(bossLives != 0)
        {
            iTween.PunchScale(otherObject.transform.parent.gameObject, new Vector3(1.1f,1.1f,1.1f), 0.5f);
            bossAnimatorState.SetBool("bossHit", true);
        }
        else if(bossLives == 0)
            StartCoroutine(BossKilled(otherObject));

        ColliderOnOff();

        if(bossLives != 0)
        {
            for(int i=0 ; i < 10 ; i++)
            {
                yield return new WaitForSeconds(0.1f);
                bossRend.enabled = !bossRend.enabled;
            }
            
            bossRend.enabled = true;
        
            if(bossLives != 0)
                ColliderOnOff();
        }
    }



    IEnumerator BossKilled(ControllerColliderHit gameObj)
    {
        PauseScript.bossDead = true;
        bossLives = 0;
        bossAgent.speed = 0;
        bossAnimatorState.SetTrigger("bossDie");
        yield return new WaitForSeconds(1f);

        GameObject go = Instantiate(explosionBoss, gameObj.transform.position, Quaternion.identity);
        Destroy(go, 2);
        GameObject go2 = Instantiate(deathBoss, gameObj.transform.position, Quaternion.identity);
        go2.transform.localScale = Vector3.one * 5;
        Destroy(go2, 2);

        Destroy(gameObj.gameObject.transform.parent.gameObject);
        audioSource.PlayOneShot(bossExplosionSound, 0.5f);
        
        goKeyMagic = Instantiate(keyMagic, gameObj.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity * Quaternion.Euler(270,0,0));
        goKeyMagic.transform.localScale  = Vector3.one * 10;
        Instantiate(key, gameObj.gameObject.transform.position, Quaternion.identity * Quaternion.Euler(0,0,0));
    }
}