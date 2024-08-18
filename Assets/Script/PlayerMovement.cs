using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public AudioClip jumpClip;
    [SerializeField] private PlayerMovementSettings settings;
    private Rigidbody2D rb;
    private new CapsuleCollider2D collider;
    private Vector2 curVelocity;
    private Vector2 hitStunVelocity;
    private bool cachedQueryStartInColliders;
    private InputData inputData;
    private float timeAC;
    private bool isInHitStun;

    //Jumping
    [SerializeField] private bool isGrounded;
    private bool coyoteOn;
    private bool jumpEndedEarly;
    private bool canJumpBuffer;
    [SerializeField]private bool jumping;
    private float jumpTime;
    private float ungroundedTime = float.MinValue;

    public struct InputData
    {
        public float horizonatal;
        public float vertical;
        public bool jumpPressed;
        public bool jumpHeld;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider= GetComponent<CapsuleCollider2D>();

        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        jumping = false;
        coyoteOn = false;
        //GlobalReferences.PLAYER.PlayerObject = this.gameObject;
    }

    private void Update()
    {
        timeAC += Time.deltaTime;
        inputData = new InputData
        {
            horizonatal = Input.GetAxisRaw("Horizontal"),
            vertical = Input.GetAxisRaw("Vertical"),
            jumpPressed = Input.GetButtonDown("Jump"),
            jumpHeld = Input.GetButton("Jump"),
        };
        if (inputData.jumpPressed)
        {
            //SoundFXManager.instance.PlaySoundFXClip(jumpClip, transform, 1f);
            jumping = true;
            jumpTime = timeAC;
        }
        
    }

    private void FixedUpdate()
    {
        CheckCollision();

        handleJump();
        movementHandler();
        gravity();

        rb.velocity = curVelocity;
    }

    private void CheckCollision()
    {

        Physics2D.queriesStartInColliders = false;
        
        bool ceilingHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.up, settings.groundedDistance, settings.GoundLayer); ;
        if (ceilingHit) curVelocity.y = Math.Min(0, curVelocity.y);

        RaycastHit2D groundHit = Physics2D.CapsuleCast(collider.bounds.center, collider.size, collider.direction, 0, Vector2.down, settings.groundedDistance, settings.GoundLayer);

        if (!isGrounded && groundHit)
        {
            isGrounded = true;
            coyoteOn = true;
            canJumpBuffer = true;
            jumpEndedEarly = false;
            isInHitStun = false;
        }
        else if (isGrounded && !groundHit)
        {
            isGrounded = false;
            ungroundedTime = timeAC;
        }

        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;

    }

    private void movementHandler()
    {
        if(inputData.horizonatal == 0)
        {
            var acc = isGrounded ? settings.groundDeceleration : settings.airDeceleration;
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, 0, acc * Time.fixedDeltaTime);
        }
        else
        {
            curVelocity.x = Mathf.MoveTowards(curVelocity.x, inputData.horizonatal * settings.maxSpeed, settings.acceleration * Time.fixedDeltaTime);
        }
    }
    private void gravity()
    {
        if (isGrounded && curVelocity.y <= 0f)
        {
            curVelocity.y = settings.groundingForce;
        }
        else
        {
            var airGrav = settings.fallAcceleration;
            if (jumpEndedEarly && curVelocity.y > 0)airGrav  *= settings.jumpEndEarlyGravityModifier;

            curVelocity.y = Mathf.MoveTowards(curVelocity.y, -settings.maxFallSpeed, airGrav * Time.fixedDeltaTime);
        }
    }

    

    private void handleJump()
    {
        //Check if player stopped holding jump button before reaching apex
        if (!jumpEndedEarly && !isGrounded && !inputData.jumpHeld && rb.velocity.y > 0) jumpEndedEarly = true;

        //Check if the Jump input was made outside of input buffer period
        if (!jumping && !(canJumpBuffer && timeAC < jumpTime + settings.jumpBuffer)) return;

        //Check if player is grounded or if coyote time is active
        if (isGrounded || (coyoteOn && !isGrounded && timeAC < ungroundedTime + settings.coyoteTime)) ExecuteJump();

        jumping = false;
    }
    private void ExecuteJump()
    {
        Debug.Log("Executing Jump");
        jumpEndedEarly = false;
        jumpTime = 0;
        canJumpBuffer = false;
        coyoteOn = false;

        curVelocity.y = settings.jumpPower;
    }
    public void bonusJump()
    {
        curVelocity.y = settings.bonusJumpPower;
    }

    public void jump()
    {
        jumping = true;
    }


    public Vector2 getCurVelocity()
    {
        return curVelocity;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setHitStunVelocity(Vector2 val)
    {
        this.hitStunVelocity = val;
    }
    public void setHitStun(bool val)
    {
        this.isInHitStun = val;
    }
    public bool getJumping()
    {
        return jumping;
    }
    public Vector2 getVelocity()
    {
        return curVelocity;
    }
}
