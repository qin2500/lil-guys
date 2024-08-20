using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //hooray level complete

        if (collision.collider.gameObject.CompareTag("Player")){
            Debug.Log("Collision with endzone detected. Level completed.");
            GlobalEvents.LevelComplete.invoke();
        }

    }
}
