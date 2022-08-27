using System.Collections;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager chkp;
    private AudioSource audioSource;

    public Vector3 lastPoint;
    public GameObject[] check;
    public Material matOff, matOn;
    public AudioClip checkSound, explosionSound;
    public GameObject checkParticules, explodeFX, splachFX;

    bool canInstanciate = true;


    void Start()
    {
        chkp = this;
        check = GameObject.FindGameObjectsWithTag("checkPoint");
        
        audioSource = GetComponent<AudioSource>();
        lastPoint = transform.position;
    }

    
    
    private void FixedUpdate()
    {
        if(PlayerInfos.pi.playerHealth <= 0 && canInstanciate)
        {
            PlayerInfos.pi.LoseLives();
            PlayerCollision.pc.rendPlayer.enabled = false;
            Explosion();

            StartCoroutine(Respawn());
        }
    }
    
    
    
    private void ResetCones()
    {
        foreach(GameObject obj in check)
        {
            obj.GetComponent<BoxCollider>().enabled = true;
            obj.GetComponent<CoinAnim>().enabled = false;
            obj.GetComponent<Renderer>().material = matOff;
        }
    }



    private void ConeCollide(GameObject check)
    {
        ResetCones();
        
        audioSource.PlayOneShot(checkSound, 0.3f);
        Instantiate(checkParticules, check.transform.position, Quaternion.identity);
        
        lastPoint = transform.position;

        check.GetComponent<BoxCollider>().enabled = false;
        check.GetComponent<CoinAnim>().enabled = true;
        check.GetComponent<Renderer>().material = matOn;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="checkPoint")
        {
            int indexCone = System.Array.IndexOf(check, other.gameObject);
            ConeCollide(check[indexCone]);
            iTween.PunchPosition(check[indexCone], Vector3.forward * 1f, 1.5f);
        }

        if(other.gameObject.tag == "water" && canInstanciate)
            StartCoroutine(RespawnWater());

        if(other.gameObject.tag == "fall" && canInstanciate)
            StartCoroutine(RespawnFall());
    }



    public void Explosion()
    {
        audioSource.PlayOneShot(explosionSound, 2);
        GameObject go = Instantiate(explodeFX, gameObject.transform.position, Quaternion.identity);
        Destroy(go, 2);
    }



    IEnumerator DisablePlayerOnGameOver()
    {
        gameObject.GetComponent<PlayerCollision>().enabled = false;
        gameObject.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(0.3f);

        gameObject.GetComponent<CharacterController>().enabled = false;
    }



    IEnumerator Respawn()
    {
        canInstanciate = false;

        yield return new WaitForSeconds(0.2f);
        
        if(PlayerInfos.pi.playerLives != 0)
        {
            yield return new WaitForSeconds(0.3f);

            transform.position = lastPoint + Vector3.up * 2;
            
            if(PlayerInfos.pi.playerHealth == 0)
            {
                PlayerCollision.pc.bossLives = 3;
                PlayerInfos.pi.SetHeatlth(3);
            }
            
            Explosion();
            canInstanciate = true;
            PlayerCollision.pc.rendPlayer.enabled = true;
        }
        else
        {
            StartCoroutine(DisablePlayerOnGameOver());
            GameOverAnimScript.gameOver.transitionLoad.SetTrigger("GameOverClose");
            PauseScript.pauseScript.pause = true;
            PauseScript.pauseScript.select = true;
        }
    }



    IEnumerator RespawnFall()
    {
        canInstanciate = false;
        CamCollision.camCol.camRotate = 2;

        yield return new WaitForSeconds(0.5f);
        
        PlayerCollision.pc.bossLives = 3;
        PlayerInfos.pi.LoseLives();

        if(PlayerInfos.pi.playerLives != 0)
        {
            transform.position = lastPoint + Vector3.up * 2;

            Explosion();
            PlayerCollision.pc.rendPlayer.enabled = true;
            PlayerInfos.pi.SetHeatlth(3);
            canInstanciate = true;
        }

        else
        {
            GameOverAnimScript.gameOver.transitionLoad.SetTrigger("GameOverClose");
            PauseScript.pauseScript.pause = true;
            PauseScript.pauseScript.select = true;
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(DisablePlayerOnGameOver());
        }
    }



    IEnumerator RespawnWater()
    {
        canInstanciate = false;
        PlayerCollision.pc.PlayerGetDamaged(-1, transform.gameObject, 0f);

        if(PlayerInfos.pi.playerHealth != 0)
        {
            yield return new WaitForSeconds(0.5f);

            transform.position = lastPoint + Vector3.up * 2;
            Explosion();
            PlayerCollision.pc.rendPlayer.enabled = true;
        }
        else
            PlayerCollision.pc.bossLives = 3;

        canInstanciate = true;
    }
}