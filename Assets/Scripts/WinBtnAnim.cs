using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinBtnAnim : MonoBehaviour
{
    public GameObject playAgainBtn, backToMainBtn, quitBtn;
    public AudioClip popSound;

    AudioSource aSource, music;


    public void Start()
    {
        playAgainBtn.transform.localScale = Vector3.zero;
        backToMainBtn.transform.localScale = Vector3.zero;
        quitBtn.transform.localScale = Vector3.zero;

        aSource = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        StartCoroutine(AnimBtnOn());
    }



    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && Input.GetAxis("Vertical") != 0)
            EventSystem.current.SetSelectedGameObject(playAgainBtn);
    }


 
    IEnumerator AnimBtnOn()
    {
        yield return new WaitForSeconds(1.2f);

        playAgainBtn.SetActive(true);
        backToMainBtn.SetActive(true);
        quitBtn.SetActive(true);

        aSource.PlayOneShot(popSound, 0.3f);
        iTween.ScaleAdd(playAgainBtn, iTween.Hash(
            "time", 0.4f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
        yield return new WaitForSeconds(0.1f);

        aSource.PlayOneShot(popSound, 0.3f);
        iTween.ScaleAdd(backToMainBtn, iTween.Hash(
            "time", 0.4f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
        yield return new WaitForSeconds(0.1f);

        aSource.PlayOneShot(popSound, 0.3f);
        iTween.ScaleAdd(quitBtn, iTween.Hash(
            "time", 0.4f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
    }


    public void Retry() => WinTransitions.wt.LoadLevel1();
    public void Back() => WinTransitions.wt.LoadLevel0();
    public void Quit() => Application.Quit();
}