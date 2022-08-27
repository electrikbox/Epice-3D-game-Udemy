using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMusic : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("music");

        if(musicObjs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        if(SceneManager.GetActiveScene().buildIndex == 0)
            gameObject.GetComponent<AudioSource>().Stop();
    }
}
