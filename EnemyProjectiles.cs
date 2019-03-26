using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyProjectiles : MonoBehaviour
{


    public float maxDistance;
    Vector2 shellStartPosition;
    Vector2 shellCurrentPosition;
    public GameObject player;
    public Vector3 direction;
    public float force;
    public GameObject explosionEffect;

    void OnEnable()
    {
        StopAllCoroutines();
        shellStartPosition = GetComponent<Transform>().position;
        shellCurrentPosition = GetComponent<Transform>().position;
        StartCoroutine("CheckTravelDistance");
    }


    IEnumerator CheckTravelDistance()
    {
        while (Vector2.Distance(shellStartPosition, shellCurrentPosition) < maxDistance)
        {
            yield return null;
            transform.position += direction * force * Time.deltaTime;
            shellCurrentPosition = GetComponent<Transform>().position;
        }

      
        StopCoroutine("CheckTravelDistance");
        kill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool explode = false;
        if (other.tag == "Obstacle")
        {
            StopCoroutine("CheckTravelDistance");
            kill();
            explode = true;
        }
        if (other.tag == "Player")
        {
            if(other.GetComponentInParent<PlayerMovement>().currentMode != PlayerMovement.PlayerMode.shellArmor)
            {
                other.GetComponentInParent<Respawn>().ResetPosition();

            }
                kill();
            explode = true;
        }

        if (explode)
        {
            Vector3 partPos = transform.position;
            partPos += new Vector3(0.0f, 0.0f, -0.5f);
            GameObject particles = Instantiate(explosionEffect, partPos, transform.rotation) as GameObject;
            Destroy(particles, 1.0f);
        }
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    public void SetAttributes(float mForce, float mMaxDistance)
    {
        force = mForce;
        maxDistance = mMaxDistance;
    }
}
