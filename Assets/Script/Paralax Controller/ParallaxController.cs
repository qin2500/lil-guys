using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxController : MonoBehaviour
{
    private float startPosx, startPosy, length, height;
    public GameObject cam;
    public Vector2 parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPosx = transform.position.x;
        startPosy = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = (Vector2)cam.transform.position * parallaxEffect;

        transform.position = new Vector3(startPosx + distance.x, startPosy + distance.y, transform.position.z);

        float movementx = cam.transform.position.x * (1 - parallaxEffect.x);

        if(movementx > startPosx + length)
        {
            startPosx += length;
        }
        else if (movementx < startPosx - length)
        {
            startPosx -= length;
        }

        float movementy = cam.transform.position.y * (1 - parallaxEffect.y);

        if (movementy > startPosy + length)
        {
            startPosy += length;
        }
        else if (movementy < startPosy - length)
        {
            startPosy -= length;
        }
    }
}
