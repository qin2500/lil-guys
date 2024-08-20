using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilGuyAnimationController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator playerAnimator;

    [SerializeField] private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {;
        if (rb.velocity.x != 0) sprite.flipX = rb.velocity.x < 0;

            if (rb.velocity.x != 0)
            {
                playerAnimator.Play("run");
            }
            else
            {
                playerAnimator.Play("idle");
            }

        

    }

}
