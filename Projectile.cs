using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public float maxDistance;
    Vector2 shellStartPosition;
    Vector2 shellCurrentPosition;
    public GameObject player;
    public Vector3 direction;
    public float force;

    void OnEnable () {
        StopAllCoroutines();
        shellStartPosition = GetComponent<Transform>().position;
        shellCurrentPosition = GetComponent<Transform>().position;
        StartCoroutine("CheckTravelDistance");
	}
	
	
	IEnumerator CheckTravelDistance()
    {
        while(Vector2.Distance(shellStartPosition,shellCurrentPosition)< maxDistance)
        {
            yield return null;
            transform.position += direction *force * Time.deltaTime;
            shellCurrentPosition = GetComponent<Transform>().position;
        }

        GetComponentInParent<ReturnPlayer>().enabled = true;
        StopCoroutine("CheckTravelDistance");
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Obstacle")
        {
            GetComponentInParent<ReturnPlayer>().enabled = true;
            StopCoroutine("CheckTravelDistance");
            enabled = false;
        }
        if (other.tag == "Enemy")
        {
            GetComponentInParent<ReturnPlayer>().enabled = true;
            GetComponentInParent<ReturnPlayer>().force = 15;
            StopCoroutine("CheckTravelDistance");
            Destroy(other.gameObject);
            enabled = false;
        }
        if (other.tag == "EnemyProjectile")
        {
            GetComponentInParent<ReturnPlayer>().enabled = true;
            GetComponentInParent<ReturnPlayer>().force = 15;
            StopCoroutine("CheckTravelDistance");
            enabled = false;

        }
    }
}
