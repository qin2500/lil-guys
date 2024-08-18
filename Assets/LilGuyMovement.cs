using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static PlayerMovement;
using System;

public class LilGuyMovement : MonoBehaviour
{
    [Header("Pathfinding")]
    public LayerMask ground;
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public float followDistance = 0.5f;

    [Header("Physics")]
    public float maxSpeed = 10f;
    public float jumpForce = 10f;
    public float acceleration = 10f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
   public float jumpCheckOffset = 0.1f;
    public float groundingForce = 10f;
    public float fallAcceleration = 5f;
    public float maxFallSpeed = 10f;
    public float deceleration = 10f;
    private Vector2 currentVelocity;
    

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;

    [SerializeField] Vector3 startOffset;

    private CapsuleCollider2D collider;
    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public bool isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;
    private bool cachedQueryStartInColliders;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;

        currentVelocity = rb.velocity;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }
    
    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            CheckCollision();
            gravity();
            PathFollow();
        }
    }
    private void CheckCollision()
    {

        Physics2D.queriesStartInColliders = false;

        bool ceilingHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.up, jumpCheckOffset, ground); 
        if (ceilingHit) currentVelocity.y = Math.Min(0, currentVelocity.y);

        RaycastHit2D groundHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.down, jumpCheckOffset, ground);

        if (!isGrounded && groundHit)
        {
            isGrounded = true;
            isJumping = false;
            isInAir = false;

        }
        else if (isGrounded && !groundHit)
        {
            isGrounded = false;
        }

        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;

    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);

        // Jump
        if(true)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return;
                isJumping = true;
                currentVelocity.y = jumpForce;
            }
        }


        // Movement
        int hoizontalInput = 1;
        if (target.position.x - rb.position.x < 0) hoizontalInput = -1;
        if(Vector2.Distance(transform.position, target.position) >followDistance)currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, hoizontalInput * maxSpeed, acceleration * Time.fixedDeltaTime);
        else currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        rb.velocity = currentVelocity;

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            flip();
        }
    }

    private void gravity()
    {
        if (isGrounded && currentVelocity.y <= 0f)
        {
            currentVelocity.y = -groundingForce;
        }
        else
        {
            var airGrav = fallAcceleration;

            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -maxFallSpeed, airGrav * Time.fixedDeltaTime);

        }
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void flip()
    {
        if (rb.velocity.x > 0.05f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < -0.05f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }


}
