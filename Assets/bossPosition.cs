using System.Collections;
using UnityEngine;

public class bossPosition : MonoBehaviour
{
    public static bossPosition bp;
    public GameObject boss;
    public GameObject[] bossHearts;



    private void Start()
    {
        bp = this;
        foreach(GameObject obj in bossHearts)
            obj.transform.localScale = Vector3.zero;
    }



    void FixedUpdate()
    {
        if(boss != null)
            transform.position = Vector3.Lerp(transform.position, boss.transform.position + new Vector3(0, 5, 0), 0.08f);
        else
            Destroy(gameObject);
    }



    public void SetBossHealthBar()
    {
        foreach(GameObject obj in bossHearts)
            if(obj != null)
            obj.GetComponent<Canvas>().enabled = false;
        
        for(int i = 0; i < PlayerCollision.pc.bossLives; i++)
            bossHearts[i].GetComponent<Canvas>().enabled = true;
    }



    public void ShowBossHearts() => StartCoroutine(BossShowHearts());

    public void HideBossHearts() => StartCoroutine(BossHideHearts());



    public IEnumerator BossShowHearts()
    {
        yield return new WaitForSeconds(0.2f);
        
        for(int i = 0; i < PlayerCollision.pc.bossLives; i++)
        {
            bossHearts[i].transform.localScale = Vector3.zero;
            bossHearts[i].GetComponent<Canvas>().enabled = true;
            iTween.ScaleTo(bossHearts[i], Vector3.one, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }



    public IEnumerator BossHideHearts()
    {
        for(int i = 0; i < 2; i++)
        {
            iTween.ScaleTo(bossHearts[i], Vector3.zero, 0.1f);
            bossHearts[i].GetComponent<Canvas>().enabled = false;
        }
        yield return new WaitForSeconds(0.1f);
    }
}
