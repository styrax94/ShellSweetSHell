using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour {

	enum JumpPlace { forward, middle, back}
    JumpPlace currentPlace;
    bool hasBeenForward;

    Vector3 startPosition;
    Vector3 landPosition;
    public float xJump;
    public float yJump;
    public float jumpCD;
    float jumpTimer;
    bool isJumping;
    public Animator EnemyAnim;
    //couritine
    float journeyLenght;
    public float speed = 15.0f;
    float startTime;
    float distCovered;
    float fracJourney;
    
	void Start () {
        hasBeenForward = false;
        currentPlace = JumpPlace.middle;
        isJumping = false;
        EnemyAnim = GetComponent<Animator>();
        EnemyAnim.SetBool("IsFacingRight", true);
        EnemyAnim.SetBool("IsFacingLeft", false);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    	if(Time.time - jumpTimer >= jumpCD && !isJumping)
             {
                StopAllCoroutines();
                isJumping = true;
                StartCoroutine("JumpEnemy");
             }
	}

    IEnumerator JumpEnemy()
    {
        startPosition = transform.position;
        landPosition = transform.position;

        if(currentPlace == JumpPlace.middle && !hasBeenForward) // jumoing to the right
        {
            

            landPosition += new Vector3(xJump, yJump, 0.0f);
           
            
        }
        else if(currentPlace == JumpPlace.middle && hasBeenForward)//left
        {
            EnemyAnim.SetBool("IsFacingRight", false);
            EnemyAnim.SetBool("IsFacingLeft", true);
            landPosition += new Vector3(-xJump, yJump, 0.0f);
           
        }
        else if(currentPlace == JumpPlace.forward) //left
        {

            EnemyAnim.SetBool("IsFacingRight", false);
            EnemyAnim.SetBool("IsFacingLeft", true);
            landPosition += new Vector3(-xJump, yJump, 0.0f);
            
            
        }
        else if(currentPlace == JumpPlace.back) //right
        {
            EnemyAnim.SetBool("IsFacingLeft", false);
            EnemyAnim.SetBool("IsFacingRight", true);
            
            landPosition += new Vector3(xJump, yJump, 0.0f);
            
        }

        journeyLenght = Vector3.Distance(startPosition, landPosition);
        startTime = Time.time;
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLenght;

        while(fracJourney <= 1)
        {
            yield return null;
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLenght;
            transform.position = Vector3.Lerp(startPosition, landPosition, fracJourney);
        }

        StopCoroutine("LandEnemy");
        StartCoroutine("LandEnemy");
    }


    IEnumerator LandEnemy()
    {
        startPosition = transform.position;

        if (currentPlace == JumpPlace.middle && !hasBeenForward)
        {

            landPosition += new Vector3(xJump, -yJump, 0.0f);
            currentPlace = JumpPlace.forward;
            hasBeenForward = true;
        }
        else if (currentPlace == JumpPlace.middle && hasBeenForward)
        {
            landPosition += new Vector3(-xJump, -yJump, 0.0f);
            currentPlace = JumpPlace.back;
            hasBeenForward = false;
        }
        else if (currentPlace == JumpPlace.forward)
        {
            landPosition += new Vector3(-xJump, -yJump, 0.0f);
            currentPlace = JumpPlace.middle;
        }
        else if (currentPlace == JumpPlace.back)
        {
            landPosition += new Vector3(xJump, -yJump, 0.0f);
            currentPlace = JumpPlace.middle;
        }
        journeyLenght = Vector3.Distance(startPosition, landPosition);
        startTime = Time.time;
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLenght;

        while (fracJourney <= 1)
        {
            yield return null;
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLenght;
            transform.position = Vector3.Lerp(startPosition, landPosition, fracJourney);
        }

        jumpTimer = Time.time;
        isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
           if(collision.GetComponentInParent<PlayerMovement>().currentMode != PlayerMovement.PlayerMode.shellArmor)
            {

                collision.GetComponentInParent<Respawn>().ResetPosition();
            }
        }
    }
}
