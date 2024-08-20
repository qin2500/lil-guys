using System.Collections.Generic;
using UnityEngine;

public class LilGuyFollowAI : MonoBehaviour
{
    public DelayedPositionUpdate target;
    private GameObject player;
    private LilGuyFollowAI follower;
    public Rigidbody2D rb;
    public float speed = 5f;
    public float followDistance = 0.01f; // Adjust this value to define how close the lil guys should be before dequeuing
    private float speedUpDistance = 5f; //Distance when lil guys start to speed up to catch up to you.
    public bool running = false;
    public float fasterSpeed = 20f;

    private Vector2 curTarget;
    private float timeAC;
    public bool head;

    public float distance;

    private bool hasJesusTakenTheWheel = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (GlobalEvents.PlayerPause.Invoked()) return;


        if (hasJesusTakenTheWheel) return;

        target.follower = this;
        timeAC += Time.deltaTime;

        //Grab new waypoint if reached current one or took too long to reach current one
        if (curTarget == Vector2.zero || Vector2.Distance(transform.position, curTarget) <= 0.3f || timeAC > 0.5f)
        {
            if (target.getPositionHistory().Count > 0)
            {
                curTarget = target.getPositionHistory().Dequeue();
            }
            timeAC = 0f;
        }

        //move towards waypoint if within follow distance
        Vector2 direction = curTarget - rb.position;
        
        if(distance < followDistance && target.positions <= 1)
        {
            target.setStopped(true);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            target.setStopped(false);
            if (!running) rb.velocity = direction.normalized * speed;
            else rb.velocity = direction.normalized * fasterSpeed;
        }

        //Tell lil guys to either run or walk
        distance = Vector2.Distance(player.transform.position, rb.position);
        if (head)
        {
            if (distance > speedUpDistance && !running) recursiveRunCommand();
            else if (distance <= speedUpDistance && running) recursiveWalkCommand();
        }

    }   
    public void recursiveRunCommand()
    {
        running = true;
        if(follower != null)
        {
            follower.recursiveRunCommand();
        }
    }
    public void recursiveWalkCommand()
    {
        running = false;
        if (follower != null)
        {
            follower.recursiveWalkCommand();
        }
    }

    public void setFollower(LilGuyFollowAI follower)
    {
        this.follower = follower;
    }
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
    public void setSpeedUpDistance(float distance)
    {
        this.speedUpDistance = distance;
    }    
    public void setHead(bool isHead)
    {
        this.head = isHead;
    }

        public void giveJesusTheWheel()
    {
        hasJesusTakenTheWheel = true;
    }

    public void takeTheWheelFromJesus()
    {
        hasJesusTakenTheWheel = false;
    }


}
