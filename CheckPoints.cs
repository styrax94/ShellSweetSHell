using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponentInParent<Respawn>().checkpoint = transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
        }

       
    }
}
