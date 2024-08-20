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
        if (this.gameObject.activeSelf && collision.gameObject.CompareTag("Death"))
        {
            this.gameObject.SetActive(false);
            GameObject death = Instantiate(deathParticle, transform.position, transform.rotation);

            death.GetComponent<Death>().die(30, () => GlobalEvents.PlayerDeath.invoke());

        }
    }
    private void playerDeath()
    {
        Debug.Log("Player is Die");
        lilGuysManager.genocide();
        this.gameObject.SetActive(false);
    }
}
