using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition transition;
    public Animator transitionLoad;

    public AudioSource audioSource;
    public AudioClip openSound, closeSound;


    private void Awake()
    {
        GameObject[] transObjs = GameObject.FindGameObjectsWithTag("transition");

        if (transObjs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }


    public void Start() => transition = this;



    public void SceneTransition(string state)
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        transitionLoad.SetTrigger(state);
    }

    public void OpenSound() => audioSource.PlayOneShot(openSound, 0.2f);

    public void CloseSound() => audioSource.PlayOneShot(closeSound, 0.2f);
}
