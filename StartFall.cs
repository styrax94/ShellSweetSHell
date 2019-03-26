using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFall : MonoBehaviour {

    public bool Active = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            foreach(FallingWall components in GetComponentsInChildren<FallingWall>())
            {
                components.StartCoroutine("Fall");
            }
            Active = true;
        }
    }

    public void ReturnWall()
    {
        foreach (FallingWall components in GetComponentsInChildren<FallingWall>())
        {
            components.StopAllCoroutines();
            components.StartCoroutine("Return");
        }


    }
}
