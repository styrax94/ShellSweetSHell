using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    // Use this for initialization
    public float moveSpeed = 5.0f;
    public Rigidbody2D shell;
    public float force;
 
    bool hasShell;
    bool lookingRight;
    bool isFalling;
    float groundedY = -5.3f;
    float groundedX;
    public Animator CharAnim;
    private bool isIdle = false;

    public AudioClip armor, throwShell, slide;
    //fallingtestingcode
    public float dashCD;
    float dashTimer;
    float currentUpdate;

    public enum PlayerMode { shellThrow, shellArmor, shellWings, shellNormal, shellSlide };
    public PlayerMode currentMode;

    private AudioSource audioSource;
    Vector2 currentRigidBodyVelocity;

    //colliders
    Vector2 normColSize;
    Vector2 normColOffset;
  
   //AirDashLimit
   public int maxAirDash;
    int currentAirDash;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //setting the audio source so the player can play sound effects

        hasShell = true;
        lookingRight = true;
        currentMode = PlayerMode.shellNormal;
        dashCD = 0.5f;
        isFalling = false;
        CharAnim = GetComponent<Animator>();
        currentUpdate = Time.time;
        normColSize = GetComponent<BoxCollider2D>().size;
          normColOffset = GetComponent<BoxCollider2D>().offset;

        dashTimer = Time.time;
        currentAirDash = maxAirDash;

    }
    void Update()
    {
        //Check if the player is airborne
        if(Time.time - currentUpdate >= 0.05f)
        {
            if (transform.position.y != groundedY)
            {
                isFalling = true;
                groundedY = transform.position.y;
                CharAnim.SetBool("NotFalling", false);
            }
            else
            {
                if(currentMode != PlayerMode.shellSlide)
                {
                    isFalling = false;
                    CharAnim.SetBool("NotFalling", true);

                    if (currentAirDash != maxAirDash)
                    {
                        currentAirDash = maxAirDash;
                    }
                  
                }            
            }

            currentUpdate = Time.time;
        }

        isIdle = true;
        //Move Left
        if (Input.GetAxisRaw("Horizontal") == -1 && currentMode != PlayerMode.shellArmor && currentMode != PlayerMode.shellSlide)
        {
            CharAnim.SetBool("IsWalkingRight", false);
            CharAnim.SetBool("IsWalkingLeft", true); 
            CharAnim.SetBool("IsIdleRight", false);
            CharAnim.SetBool("IsIdleLeft", true);
            isIdle = false;        
           transform.position = transform.position += transform.right * -moveSpeed * Time.deltaTime;
            //GetComponentInParent<Rigidbody2D>().MovePosition(transform.position + ((Vector3.down * 9.81f*Time.fixedDeltaTime) + (transform.right * -moveSpeed * Time.fixedDeltaTime)));
            lookingRight = false;
        }
        //Move Right
        else if (Input.GetAxisRaw("Horizontal") == 1 && currentMode != PlayerMode.shellArmor && currentMode != PlayerMode.shellSlide)
        {
            CharAnim.SetBool("IsWalkingRight", true);
            CharAnim.SetBool("IsWalkingLeft", false);
            CharAnim.SetBool("IsIdleRight", true);
            CharAnim.SetBool("IsIdleLeft", false);
            isIdle = false;
          
            
           transform.position = transform.position += transform.right * moveSpeed * Time.deltaTime;
          

           
            lookingRight = true;
           
        }



        if (Input.GetButton("Shoot") && hasShell && currentMode == PlayerMode.shellNormal)
        {
            hasShell = false;
            currentMode = PlayerMode.shellThrow;
            isIdle = false;
            if (lookingRight)
            {
                CharAnim.Play("ThrowShellV2");
            }
            else CharAnim.Play("ThrowShellLeftV2");
        }
        if (Input.GetButton("Armor") && currentMode == PlayerMode.shellNormal)
        {
            currentMode = PlayerMode.shellArmor;
            audioSource.PlayOneShot(armor, 2.0f);
            ReduceColliders();
            isIdle = false;

            CharAnim.SetBool("NotArmoured", false);
            if (lookingRight) { CharAnim.Play("Armour"); }
            else { CharAnim.Play("ArmourLeft"); }
        }
        if(Input.GetButtonUp("Armor") && currentMode == PlayerMode.shellArmor)
        {

            ResetColliders();
            CharAnim.SetBool("NotArmoured", true);
        }
        if(Input.GetButton("Float") && isFalling && currentMode == PlayerMode.shellNormal)
        {
            if (lookingRight) { CharAnim.Play("FallingV2"); }
            else { CharAnim.Play("FallingLeftV2"); }
            GetComponentInParent<Rigidbody2D>().gravityScale = 0.4f;
            currentMode = PlayerMode.shellWings;
            ReduceColliders();
            isIdle = false;

        }
        if (Input.GetButtonUp("Float") && currentMode == PlayerMode.shellWings)
        {
           
            GetComponentInParent<Rigidbody2D>().gravityScale = 1.0f;
            ResetColliders();
            CharAnim.SetBool("NotFalling", true);
        }
        if(Input.GetButton("Dash")&& currentMode == PlayerMode.shellNormal && Time.time - dashTimer >= dashCD)
        {
            if ((isFalling && currentAirDash > 0) || !isFalling)
            {
                if (lookingRight) { CharAnim.Play("SlidingV2"); }
                else { CharAnim.Play("SlidingLeftV2"); }
                currentMode = PlayerMode.shellSlide;
                StopAllCoroutines();
                StartCoroutine("ShellSlide");
                currentRigidBodyVelocity = GetComponentInParent<Rigidbody2D>().velocity;

                audioSource.PlayOneShot(slide, 2.0f); //play slide sound

                ReduceColliders();

                isIdle = false;
                if(isFalling)
                currentAirDash--;
            }     
        }

     

        if (isIdle)
        {
            CharAnim.SetBool("IsWalkingRight", false);
            CharAnim.SetBool("IsWalkingLeft", false);
        }
    }

    void FixedUpdate()
    {
        //sets the velocity when in winged mode
        if (currentMode == PlayerMode.shellWings)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, (GetComponent<Rigidbody2D>().velocity.y * 95)/100);
        }

        
      
    }
    public void SetHasShell(bool shell)
    {
        hasShell = shell;
        currentMode = PlayerMode.shellNormal;
    }

   

   IEnumerator ShellSlide()
    {
        Vector3 startMarker = transform.position;
        Vector3 endMarker = transform.position;



        if (lookingRight)
        {
            endMarker += new Vector3(8.0f, 0.0f, 0.0f);
            CharAnim.SetBool("IsIdleRight", true);
            CharAnim.SetBool("IsIdleLeft", false);
        }
        else
        {
            endMarker += new Vector3(-8.0f, 0.0f, 0.0f);
            CharAnim.SetBool("IsIdleRight", false);
            CharAnim.SetBool("IsIdleLeft", true);
        }

      

        float journeyLength = Vector3.Distance(startMarker, endMarker);
        float speed = 15.0f;
        float startTime = Time.time;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        while (fracJourney <= 1)
        {
            yield return null;
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            //  Debug.Log(fracJourney);
            GetComponentInParent<Rigidbody2D>().MovePosition(Vector3.Lerp(startMarker, endMarker, fracJourney));
           // transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
        }

        EndSlideCouritine();
    }
   
   public void EndSlideCouritine()
    {
        StopCoroutine("ShellSlide");
        GetComponentInParent<Rigidbody2D>().velocity = currentRigidBodyVelocity;
        ResetColliders();
        dashTimer = Time.time;
    }
  
   public void Fire()
    {
        Vector3 shellPosition = transform.position;
        shellPosition += new Vector3(0.0f, 0.5f, 0.0f);
        Rigidbody2D proj = Instantiate(shell, shellPosition, transform.rotation) as Rigidbody2D;

        audioSource.PlayOneShot(throwShell, 2.0f); //play throw sound

        if (lookingRight)
        {
            proj.GetComponentInParent<Projectile>().direction = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else proj.GetComponentInParent<Projectile>().direction = new Vector3(-1.0f, 0.0f, 0.0f);

    }


    public void ResetColliders()
    {
       /* GetComponent<BoxCollider2D>().size = normColSize;
        GetComponent<BoxCollider2D>().offset = normColOffset;
      */
        currentMode = PlayerMovement.PlayerMode.shellNormal;
        
    }

   private void ReduceColliders()
    {
        /*
        GetComponent<BoxCollider2D>().size += new Vector2(0.0f, -1.1f);
        GetComponent<BoxCollider2D>().offset += new Vector2(0.0f, -0.55f);
        */ 

    }

    public void RestartVariables()
    {
        isFalling = false;
        //isIdle = true;
        CharAnim.Play("CharacterIdleV2");
    }
}

    
