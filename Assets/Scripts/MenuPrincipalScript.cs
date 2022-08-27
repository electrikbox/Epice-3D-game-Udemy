using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPrincipalScript : MonoBehaviour
{
    PlayerControls controls;

    public GameObject menu, commands, options, startButton, retourButtonCommands, retourButtonOptions, commandsButton, optionsButton;
    private AudioSource music, menuMusic;
    private Scene scene;

    Vector2 moveMenu;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Menu.moveMenu.performed += ctx => moveMenu = ctx.ReadValue<Vector2>();
        controls.Menu.moveMenu.canceled += ctx => moveMenu = Vector2.zero;
    }



    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        menuMusic = GetComponent<AudioSource>();
    }



    public void LoadLevel1() => StartCoroutine("loadSceneDly");
    public void BackMainMenu() => StartCoroutine("BackToMain");
    public void QuitGame() => Application.Quit();

    public void Commands() => StartCoroutine(loadCommands(false, retourButtonCommands));
    public void RetourCommands() => StartCoroutine(loadCommands(true, commandsButton));
    
    public void Options() => StartCoroutine(loadOptions(false, retourButtonOptions));
    public void RetourOptions() => StartCoroutine(loadOptions(true, optionsButton));


    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && moveMenu != Vector2.zero)
        {
            BtnSounds.bs.Click();
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }

    IEnumerator loadSceneDly()
    {
        Transition.transition.SceneTransition("Close");
        menuMusic.Stop();
        yield return new WaitForSeconds(0.4f);
        // music.Play();
        // SceneManager.LoadScene(1);
        SceneLoader.sl.LoadLevel(1);
        music.Play();
    }



    IEnumerator BackToMain()
    {
        Transition.transition.SceneTransition("Close");
        yield return new WaitForSeconds(0.5f);
        // SceneManager.LoadScene(0);
        SceneLoader.sl.LoadLevel(0);
    }



    IEnumerator loadCommands(bool active, GameObject button)
    {
        yield return new WaitForSeconds(0.2f);
        menu.SetActive(active);
        commands.SetActive(!active);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }



    IEnumerator loadOptions(bool active, GameObject button)
    {
        yield return new WaitForSeconds(0.2f);
        menu.SetActive(active);
        options.SetActive(!active);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
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
