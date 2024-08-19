using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour, Damageable 
{
    [SerializeField] private int health;
    [SerializeField] private GameObject destroyParticleEffect;
    public void takeDamage(int damage)
    {
        if (health > 0) health -= damage;


        if (health <= 0)
        {
            Instantiate(destroyParticleEffect, this.gameObject.transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
