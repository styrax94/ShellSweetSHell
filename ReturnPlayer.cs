using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPlayer : MonoBehaviour {


  
    Vector2 shellCurrentPosition;
    public Transform player;
    public Vector3 direction;
    public float force;
   


    void OnEnable()
    {
        StopAllCoroutines();
        shellCurrentPosition = GetComponent<Transform>().position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("ReturnToPlayer");
       
    }


    IEnumerator ReturnToPlayer()
    {
        //Debug.Log(Vector2.Distance(player.position, shellCurrentPosition));
        while (Vector2.Distance(player.position, shellCurrentPosition) > 0.2f)
        {
          //  Debug.Log("IM here :2");
            yield return null;
          //  Debug.Log("IM here :3");
            direction = player.position - transform.position;
            direction = direction.normalized;
            Debug.Log(direction);
            transform.position += direction * force * Time.deltaTime;
            shellCurrentPosition = GetComponent<Transform>().position;
        }

        player.GetComponentInParent<PlayerMovement>().SetHasShell(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
           
            Destroy(other.gameObject);
            
        }
       
    }
}
