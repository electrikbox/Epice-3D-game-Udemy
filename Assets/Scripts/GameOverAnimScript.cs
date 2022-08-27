using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverAnimScript : MonoBehaviour
{
    public static GameOverAnimScript gameOver;
    public GameObject gameOverText, retryBtn, backBtn;
    public Animator transitionLoad;
    public AudioClip popSound, boomSound, gameOverMusic;

    AudioSource music, aSource;


    public void Start()
    {
        gameOver = this;
        aSource = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
    }



    private void Update()
    {
        // if(EventSystem.current.currentSelectedGameObject == null && Input.GetAxis("Vertical") != 0)
        //     EventSystem.current.SetSelectedGameObject(retryBtn);
    }

    private void PlayGameOverMusic()
    {
        music.Stop();
        aSource.PlayOneShot(boomSound, 0.3f);
        aSource.PlayOneShot(gameOverMusic, 0.4f);
    }

    public void PlayMusic() => music.Play();
 

 
    IEnumerator AnimBtnOn()
    {
        iTween.ScaleAdd(gameOverText, iTween.Hash(
            "time", 0.5f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
        yield return new WaitForSeconds(0.1f);

        aSource.PlayOneShot(popSound, 0.5f);
        iTween.ScaleAdd(retryBtn, iTween.Hash(
            "time", 0.4f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
        yield return new WaitForSeconds(0.1f);

        aSource.PlayOneShot(popSound, 0.5f);
        iTween.ScaleAdd(backBtn, iTween.Hash(
            "time", 0.4f,
            "amount", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic
        ));

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(retryBtn);
    }



    public void Retry() => SceneLoader.sl.LoadLevel(1);

    public void Back() => StartCoroutine("BackToMainMenu");



    IEnumerator BackToMainMenu()
    {
        PauseScript.pauseScript.gameOver = true;
        Transition.transition.transitionLoad.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        // SceneManager.LoadScene(0);
        SceneLoader.sl.LoadLevel(0);
        PauseScript.pauseScript.gameOver = true;
    }



    public void BtnOff()
    {
        gameOverText.SetActive(false);
        retryBtn.SetActive(false);
        backBtn.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
