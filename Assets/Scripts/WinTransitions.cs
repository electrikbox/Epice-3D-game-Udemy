using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class WinTransitions : MonoBehaviour
{
    PlayerControls controls;

    public static WinTransitions wt;
    public GameObject pauseRetourButton;
    private AudioSource music;
    
    Vector2 moveMenu;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Menu.moveMenu.performed += ctx => moveMenu = ctx.ReadValue<Vector2>();

        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
                GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("wintransition");

        if(musicObjs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }



    private void Start() => wt = this;



    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && moveMenu != Vector2.zero)
            EventSystem.current.SetSelectedGameObject(pauseRetourButton);
    }



    public void LoadLevel0() => StartCoroutine(Load(0));
    public void LoadLevel1() => StartCoroutine(Load(1));
 
 

    IEnumerator Load(int level)
    {
        Transition.transition.transitionLoad.SetTrigger("Open");
        yield return new WaitForSeconds(0.5f);
        // SceneManager.LoadScene(level);
        SceneLoader.sl.LoadLevel(level);
        if(level != 0)
            music.Play();
    }




    void OnEnable()
    {
        controls.Menu.Enable();
    }

    void OnDisable()
    {
        controls.Menu.Disable();
    }
}
