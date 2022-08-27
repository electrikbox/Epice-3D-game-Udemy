using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class CamCollision : MonoBehaviour
{
    public static CamCollision camCol;
    public GameObject cam;
    public CinemachineVirtualCamera vcam1, vcam2, vcam3, vcam4;
    public PostProcessVolume ppv;
    
    public int camRotate = 0;
    bool coroutinePorte = true;
    bool coroutineKey = true;
    bool navON = true;

    Vector3[] camLook = new [] { new Vector3(-90f, -41.975f, 0f), new Vector3(0f, -41.975f, 90f), new Vector3(90f, -41.975f, 0f), new Vector3(0f, -41.975f, -90f) };
    PlayerController cc;
    Animator anim;
    GameObject[] mobNavs;
    NavMeshAgent mobAgent;

    public DepthOfField depthOfField;
    


    private void Start()
    {
        camCol = this;
        ppv.sharedProfile.TryGetSettings<DepthOfField>(out depthOfField);
        vcam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 19;
        cc = gameObject.GetComponent<PlayerController>();
        anim = gameObject.GetComponent<Animator>();
    }



    private void LateUpdate()
    {
        CamRotate();

        if(PlayerInfos.pi.nbPoussin == 3 && coroutinePorte)
        {
            coroutinePorte = false;
            StopPlayer();
            OpenDoor.openDoor.Open();
            StartCoroutine(SwitchTest("porte", "main"));
        }

        if(PlayerInfos.pi.gotKey && coroutineKey)
        {
            coroutineKey = false;
            StopPlayer();
            StartCoroutine(SwitchTest("key", "boss"));
        }
    }



    private void StopPlayer()
    {
        cc.enabled = false;
        anim.enabled = false;
        PlayerCollision.pc.isInvincible = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        for(int i = 1; i < 5; i++)
            if(other.gameObject.tag == "camRotate" + i.ToString())
                camRotate = i;

        if(other.gameObject.tag == "zoom")
        {
            SwitchPriority("boss");
            SetDepthOfField(50f);
        }
    }


    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "camRotate1"  && camRotate != 4 || other.gameObject.tag == "camRotate3" && camRotate != 4)
            camRotate = 2;

        if(other.gameObject.tag == "zoom")
        {
            SwitchPriority("main");
            SetDepthOfField(10.5f);
        }
    }



    public void SetDepthOfField(float nb) => depthOfField.focusDistance.value = nb;



    public void CamRotate()
    {
        for(int i = 0; i < camLook.Length; i++)
            if(camRotate == i + 1)
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.LookRotation(camLook[i]), 5f * Time.deltaTime);
    }



    private void SwitchPriority(string name)
    {
        switch(name)
        {
            case "main":
                vcam1.Priority = 2;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                break;

            case "porte":
                vcam1.Priority = 0;
                vcam2.Priority = 2;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                break;

            case "boss":
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 2;
                vcam4.Priority = 0;
                break;

            case "key":
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 2;
                break;
        }
    }



    private void MobNavSwitch() 
    {
        mobNavs = GameObject.FindGameObjectsWithTag("mobObject");

        foreach(GameObject mobNav in mobNavs)
        {
            mobAgent = mobNav.GetComponent<NavMeshAgent>();

            if(navON)
                mobAgent.isStopped = true;
            else
                mobAgent.isStopped = false;
        }
        navON = !navON;
    }



    IEnumerator SwitchTest(string tag1, string tag2)
    {
        MobNavSwitch();
        yield return new WaitForSeconds(0.3f);

        SwitchPriority(tag1);
        yield return new WaitForSeconds(3.0f);

        SwitchPriority(tag2);
        // cc.enabled = true;
        // anim.enabled = true;
        yield return new WaitForSeconds(0.5f);

        cc.enabled = true;
        anim.enabled = true;

        MobNavSwitch();
        PlayerCollision.pc.isInvincible = false; 
    }
}