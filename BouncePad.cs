using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {

    public float speed;
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.GetComponent<PlayerMovement>().currentMode == PlayerMovement.PlayerMode.shellSlide)
            {
                collision.GetComponent<PlayerMovement>().EndSlideCouritine();
            }
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed);
        }
    }
}
