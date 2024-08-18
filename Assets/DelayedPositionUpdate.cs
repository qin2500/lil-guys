using System.Collections.Generic;
using UnityEngine;

public class DelayedPositionUpdate : MonoBehaviour
{
    [SerializeField] private float delayTime = 1f;
    [SerializeField] private float updateFrequency = 0.1f;

    public Queue<Vector2> positionHistory = new Queue<Vector2>();
    private float elapsedTime = 0.0f;
    private Vector2 lastRecordedPosition;

    private void Start()
    {

        lastRecordedPosition = transform.position;
        positionHistory.Enqueue(lastRecordedPosition);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateFrequency)
        {
            Vector2 currentPosition = transform.position;


            if (currentPosition != lastRecordedPosition)
            {
                positionHistory.Enqueue(currentPosition);
                lastRecordedPosition = currentPosition;
            }

            elapsedTime = 0.0f;
        }


        while (positionHistory.Count > 0 && positionHistory.Count > delayTime / updateFrequency)
        {
            positionHistory.Dequeue();
        }
    }

    public Vector2 GetDelayedPosition()
    {
        if (positionHistory.Count > 0)
        {
            return positionHistory.Peek();
        }
        else
        {
            return transform.position; 
        }
    }

    public Queue<Vector2> getPositionHistory()
    {
        return positionHistory;
    }
}
