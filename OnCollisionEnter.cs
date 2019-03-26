using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionEnter : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player" && other.collider.GetComponent<PlayerMovement>().currentMode != PlayerMovement.PlayerMode.shellArmor)
        {
            other.collider.GetComponent<Respawn>().ResetPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Respawn>().ResetPosition();
           
            if (GetComponentInParent<StartFall>())
            {

                GetComponentInParent<StartFall>().ReturnWall();
            
            }      
        }
    }

}
