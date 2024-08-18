using System.Collections.Generic;
using UnityEngine;

public class LilGuyFollowAI : MonoBehaviour
{
    public DelayedPositionUpdate target;
    public Rigidbody2D rb;
    public float speed = 5f;
    public float followDistance = 0.1f; // Adjust this value to define how close the AI should be before dequeuing

    private Vector2 curTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (curTarget == Vector2.zero || Vector2.Distance(transform.position, curTarget) <= followDistance)
        {
            if (target.getPositionHistory().Count > 0)
            {
                curTarget = target.getPositionHistory().Dequeue();
            }
        }

        Vector2 direction = curTarget - rb.position;

        if (direction.magnitude > followDistance)
        {
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
}
