using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public GameObject explosionEffect;
    public Vector3 checkpoint;

    private AudioSource audioSource;
    public AudioClip respawnSound;

    //ForFallingWalls
    public GameObject fallingWalls;

    public int Score;

    void Start () {
        Score = 1;
        audioSource = GetComponent<AudioSource>(); //setting the audio source so the player can play sound effects
        checkpoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetPosition()
    {
        audioSource.PlayOneShot(respawnSound, 2.0f);

        GetComponentInParent<PlayerMovement>().StopAllCoroutines();
        GetComponentInParent<PlayerMovement>().currentMode = PlayerMovement.PlayerMode.shellNormal;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponentInParent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        foreach (BoxCollider2D collider in GetComponentsInParent<BoxCollider2D>())
        {
            collider.enabled = false;
        }
        GetComponentInParent<PlayerMovement>().enabled = false;
       
        GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        Vector3 partPos = transform.position;
        partPos += new Vector3(0.0f, 0.0f, -0.5f);
        GameObject particles = Instantiate(explosionEffect, partPos, transform.rotation) as GameObject;
        Destroy(particles, 1.0f);

        //ResetWallPosition
        if (fallingWalls.GetComponent<StartFall>().Active)
        fallingWalls.GetComponent<StartFall>().ReturnWall();

        StopCoroutine("RespawnPlayer");
        StartCoroutine("RespawnPlayer");
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        transform.position = checkpoint;
        GetComponentInParent<SpriteRenderer>().enabled = true;
        foreach (BoxCollider2D collider in GetComponentsInParent<BoxCollider2D>())
        {
            collider.enabled = true;
        }
        GetComponentInParent<PlayerMovement>().enabled = true;
       
        GetComponentInParent<PlayerMovement>().ResetColliders();

        GetComponentInParent<PlayerMovement>().RestartVariables();
        GetComponentInParent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
        Score++;
    }
}
