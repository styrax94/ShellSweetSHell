using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSlide : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            if(collision.collider.GetComponent<PlayerMovement>().currentMode == PlayerMovement.PlayerMode.shellSlide)
            {
                collision.collider.GetComponent<PlayerMovement>().EndSlideCouritine();
            }
        }

    }
}
