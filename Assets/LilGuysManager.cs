using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LilGuysManager : MonoBehaviour
{
    public int lilGuysCount = 5;

    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement= GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lilGuysCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerMovement.bonusJump();
                lilGuysCount--;
            }
        }

    }
}
