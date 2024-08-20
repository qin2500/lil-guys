using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LilGuysManager lilGuysManager;
    [SerializeField] private GameObject deathParticle;
    void Start()
    {
        lilGuysManager = GetComponent<LilGuysManager>();      
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Death"))
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
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
