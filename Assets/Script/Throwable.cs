using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    private Rigidbody2D rb;
    private float throwPower = 0;
    private LayerMask ground;
    private Quaternion initialRotation;

    public float timeToDisable;
    [SerializeField] public GameObject collisionParticleEffect;

    // Start is called before the first frame update


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator ApplyInitialForce()
    {
        Debug.Log("Applying initialForce");
        yield return null;
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * throwPower, ForceMode2D.Impulse);
    }


    public void throwObject()
    {
        Debug.Log("Throw object called");
        Invoke("killObject", timeToDisable);
        initialRotation = transform.rotation;
        StartCoroutine(ApplyInitialForce());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (throwPower == 0) return;

        Instantiate(collisionParticleEffect, this.gameObject.transform.position, Quaternion.identity);

        killObject();
  

            if (collision.collider.gameObject.CompareTag("Damageable"))
            {
                Damageable damageable = collision.collider.GetComponent<Damageable>();

                if (damageable != null)
                {
                    damageable.takeDamage(1); //TODO: make this variable if necessary
                }
            }
    }


    public void triggerThrow(float power, float timeToDisable) 
    {
        throwPower = power;
        this.timeToDisable = timeToDisable;
        throwObject();
    }

    private void killObject()
    {
        Destroy(this.gameObject);
        //idea - maybe should make a destroyable script to give each object callable custom destroy code
        //TODO - do the above idea
    }


}
