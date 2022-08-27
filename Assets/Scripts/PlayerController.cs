using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    private CharacterController cc;

    private float moveSpeed;
    public float jumpForce;
    public float playerGravity = 30;
    public bool collide;

    public Vector3 moveDir, velocity, localMove;
    private Animator anim;
    private AudioSource audioSource;

    bool isWalking = false;
    bool isJumping = false;
    public bool speedPressed = false;

    public Transform camTrans;
    public AudioClip jumpSound, jump2, ouch;

    GameObject inputObj;
    Vector2 mov;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Jump.performed += ctx => isJumping = true;
        controls.Player.Jump.canceled += ctx => isJumping = false;

        controls.Player.Move.performed += ctx => mov = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => mov = Vector2.zero;

        controls.Player.MoveSpeed.performed += ctx => speedPressed = true;
        controls.Player.MoveSpeed.canceled += ctx => speedPressed = false;
    }



    private void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }



    private void FixedUpdate()
    {
        PlayerLook();

        if(cc.isGrounded)
        {   
            velocity.y = -1f;
            playerGravity = 30;
        }

        // fall faster
        if(velocity.y < -1)
            playerGravity = 50;
   
        MoveFaster();

        Debug.DrawRay(cc.transform.position, Vector3.down * 0.2f, Color.green);
    }



    void Update()
    {
        if(isJumping && IsGrounded())
        {   
            isJumping = false;
            audioSource.PlayOneShot(jumpSound, 0.3f);
            Jump();
        }
        else if(collide)
        {
            collide = false;
            Jump();
        }
        else
            velocity.y -= playerGravity * 2.5f * Time.deltaTime;

        if(gameObject.GetComponent<CharacterController>().enabled)
            MovePlayer();
    }



    public void MovePlayer()
    {
        //direction camera
        Vector3 lookDir = camTrans.forward;
        lookDir.y = 0;
        lookDir = lookDir.normalized;

        Vector3 right = camTrans.right;
        right.y = 0;
        right = right.normalized;

        if(mov.x < 0.3f && mov.x > 0)
            mov.x = 0;
        if(mov.x > -0.3f && mov.x <0)
            mov.x = 0;

        if(mov.y < 0.3f && mov.y > 0)
            mov.y = 0;
        if(mov.y > -0.3f && mov.y <0)
            mov.y = 0;

        //X et Y en fonction de la rotation de la camera
        localMove.x = (mov.x * right.x) + (mov.y * lookDir.x);
        localMove.z = (mov.x * right.z) + (mov.y * lookDir.z);
        
        //bouger le player
        moveDir = new Vector3(localMove.x, 0, localMove.z);
        moveDir.Normalize();

        if(gameObject.GetComponent<CharacterController>().enabled)
        {
            cc.Move(moveDir * moveSpeed * Time.deltaTime);
            cc.Move(velocity * Time.deltaTime);
        }
    }


    
    public void Jump()
    {
        playerGravity = 37.0f;
        velocity.y = jumpForce;
    }

    
    
    void MoveFaster()
    {
        if(speedPressed)
        {
            moveSpeed = 10f;
            anim.speed = 1.7f;
        }
        else
        {
            moveSpeed = 8f;
            anim.speed = 1f;
        }
    }

    
    
    void PlayerLook()
    {
        anim.SetBool("isWalking", isWalking);

        if(moveDir.x != 0 || moveDir.z != 0)
        {
            isWalking = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x * Time.fixedDeltaTime, 0, moveDir.z * Time.fixedDeltaTime)), 0.02f);
    
        }
        else
            isWalking = false;
    }


    //AutoJump
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!cc.isGrounded)
            switch (hit.gameObject.tag)
            {
                case "mob":
                    AutoJump(ouch, 0.5f, 15);
                    break;

                case "test":
                    AutoJump(jump2, 0.3f, 30);
                    break;

                case "bossKill":
                    AutoJump(ouch, 0.5f, 25);
                    break;
            }
    }



    private void AutoJump(AudioClip clip, float vol, int force)
    {
        audioSource.PlayOneShot(clip, vol);
        jumpForce = force;
        StartCoroutine(StopJump());
        collide = true;
    }



    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(0.04f);
        jumpForce = 23;
    }



    private bool IsGrounded()
    {
        if(cc.isGrounded)
            return true;

        if(Physics.Raycast(cc.transform.position, Vector3.down, 0.2f))
            return true;
    
        else
            return false;
    }



    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();
}