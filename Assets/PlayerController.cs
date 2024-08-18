using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LilGuysManager lilGuysManager;
    void Start()
    {
        lilGuysManager = GetComponent<LilGuysManager>();      
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Death"))
        {
            playerDeath();
        }
    }
    private void playerDeath()
    {
        Debug.Log("Player is Die");
        lilGuysManager.genocide();
        this.gameObject.SetActive(false);
    }
}
