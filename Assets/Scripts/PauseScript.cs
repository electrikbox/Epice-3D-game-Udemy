using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    PlayerControls controls;

    public static PauseScript pauseScript;
    public GameObject menuPause, pauseRetourButton;
    public Camera mapCam;

    private AudioSource music;
    public AudioSource audioSource;
    public AudioClip pauseIn, pauseOut;

    public PlayerController player;

    public Text objectifs, boss;

    bool isPaused = false;
    public bool pausePressed = false;
    public bool selectPressed = false;

    public bool gameOver = false;
    public bool select = false;
    public bool pause = false;
    
    public static int amisRestants = 3;
    public static bool bossDead = false;

    Vector2 moveMenu;


    private void Awake()
    {
        controls = new PlayerControls();

        controls.Menu.moveMenu.performed += ctx => moveMenu = ctx.ReadValue<Vector2>();
        controls.Menu.moveMenu.canceled += ctx => moveMenu = Vector2.zero;

        controls.Menu.Pause.performed += ctx => pausePressed = true;
        controls.Menu.Select.performed += ctx => selectPressed = true;
        controls.Menu.Pause.canceled += ctx => pausePressed = false;
        controls.Menu.Select.canceled += ctx => selectPressed = false;        
    }



    private void Start()
    {
        pauseScript = this;
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
    }

    public void SetObjectifTxt()
    {
        Color color = Color.magenta;

        objectifs.text = $"- There are still <color=orange>{amisRestants}</color> friends to free.";

        if(amisRestants == 1)
            objectifs.text = $"- There are still <color=orange>{amisRestants}</color> friend to free.";
        if(amisRestants == 0)
            objectifs.text = "<color=orange>- You've freed all your friends!</color>";

        if(!bossDead)
            boss.text = "- You <color=red>have to kill</color> the Boss.";
        if(bossDead)
            boss.text = "<color=orange>- You killed the Boss !</color>";
    }



    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && moveMenu != Vector2.zero)
            EventSystem.current.SetSelectedGameObject(pauseRetourButton);

        if(!gameOver)
        {
            if(pausePressed && !pause)
            {
                pausePressed = false;
                Pause();
            }

            if(selectPressed && !select)
            {
                selectPressed = false;
                Map();
            }
        }
    }



    private void FixedUpdate()
    {
        if(PlayerInfos.pi.playerLives == 0)
        {
            ResetPlayer();
        }
    }



    public void Pause()
    {
        if(isPaused)
        {
            music.Play();
            PauseSwitch(pauseOut, 1f);
        }
        else
        {
            music.Pause();
            PauseSwitch(pauseIn, 0f);

            SetObjectifTxt();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseRetourButton);
        }
    }



    public void Map()
    {
        if(isPaused)
            MapSwitch(pauseOut, 1f);

        else
            MapSwitch(pauseIn, 0f);
    }



    private void PauseSwitch(AudioClip clip, float timeSpeed)
    {
        select = !select;
        audioSource.PlayOneShot(clip);
        isPaused = !isPaused;
        menuPause.SetActive(isPaused);
        Time.timeScale = timeSpeed;
        player.enabled = !player.enabled;
    }



    private void MapSwitch(AudioClip clip, float timeSpeed)
    {
        pause = !pause;
        audioSource.PlayOneShot(clip);
        isPaused = !isPaused;
        Time.timeScale = timeSpeed;
        player.enabled = !player.enabled;

        mapCam.enabled = !mapCam.enabled;
        CamCollision.camCol.depthOfField.active = !CamCollision.camCol.depthOfField.active;
        RenderSettings.fog = !RenderSettings.fog;
        
    }
    


    public void LoadLevel0() => StartCoroutine(RetourenuPrincipal(0));

    public void LoadLevel1() => StartCoroutine(RetourenuPrincipal(1));

    public void backToMain() => StartCoroutine(RetourenuPrincipalPause(0));

    public void LoadWinScene() => StartCoroutine(RetourenuPrincipalPause(2));



    private void ResetPlayer()
    {
        gameOver = true;

        amisRestants = 3;
        bossDead = false;
        
        select = false;
        menuPause.SetActive(isPaused);
        isPaused = false;
        
        Time.timeScale = 1f;
        player.enabled = true;
    }



    IEnumerator RetourenuPrincipal(int level)
    {        
        yield return new WaitForSeconds(0.01f);
        ResetPlayer();
        
        // SceneManager.LoadScene(level);
        SceneLoader.sl.LoadLevel(level);
        gameOver = false;
    }



    IEnumerator RetourenuPrincipalPause(int level)
    {
        Transition.transition.transitionLoad.SetTrigger("Close");
        ResetPlayer();
        
        yield return new WaitForSeconds(0.5f);
        
        // SceneManager.LoadScene(level);
        SceneLoader.sl.LoadLevel(level);
        gameOver = false;
        
        music.Stop();
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