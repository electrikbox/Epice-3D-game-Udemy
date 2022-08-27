using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader sl;
    public GameObject LoadingScreen;
    public Text progressionText;
    Animator anim;



    private void Awake()
    {
        GameObject[] loadObjects = GameObject.FindGameObjectsWithTag("load");

        if(loadObjects.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }



    private void Start()
    {
        sl = this;
        anim = GameObject.FindGameObjectWithTag("transition").GetComponent<Animator>();
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }



    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progress = progress *100;
            progressionText.text = "loading " + progress.ToString() + "%";
            yield return null;
        }

        anim.SetBool("isloaded", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isloaded", false);
        LoadingScreen.SetActive(false);
    }
}
