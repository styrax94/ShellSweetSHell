using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public Rigidbody2D shell;
    public float force;
    public float maxDistanceBullet;
    public float shootCD;
    public Transform player;
    float lastShot;
    public float distanceToShoot;
    public bool lookingRight;

    void Start () {
        lastShot = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

       
        if (Vector2.Distance(player.position,transform.position) < distanceToShoot)
            {
                if(Time.time- lastShot >= shootCD)
                 {
               
                   fire();
                   lastShot = Time.time;
                 }
            }
	}

    private void fire()
    {

        Rigidbody2D proj = Instantiate(shell, transform.position, transform.rotation) as Rigidbody2D;
        Physics2D.IgnoreCollision(proj.GetComponentInParent<BoxCollider2D>(), GetComponentInParent<BoxCollider2D>());

        if (lookingRight)
        {
            proj.GetComponentInParent<EnemyProjectiles>().direction = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else proj.GetComponentInParent<EnemyProjectiles>().direction = new Vector3(-1.0f, 0.0f, 0.0f);

        proj.GetComponentInParent<EnemyProjectiles>().SetAttributes(force, maxDistanceBullet);
       
    }
}
