using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LilGuysManager : MonoBehaviour
{
    [SerializeField]private GameObject lilGuy;
    [SerializeField]private GameObject lilGuySpawn;
    [SerializeField] private DelayedPositionUpdate positionUpdater;
    [SerializeField] private float lilGuysCatchUpDistance = 3f;//Distance when lil guys start to speed up to catch up to you.

    private int lilGuysCount = 0;
    private LinkedList<GameObject> lilGuys = new LinkedList<GameObject>();

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

        if(Input.GetKeyDown(KeyCode.E))
        {
            summonBoy();
        }
    }

    public GameObject summonBoy()
    {
        GameObject spawnedObject = Instantiate(lilGuy, lilGuySpawn.transform.position, Quaternion.identity);        

        LilGuyFollowAI lilGuyAi = spawnedObject.GetComponent<LilGuyFollowAI>();
        if (lilGuys.Count != 0)
        {
            lilGuys.Last.Value.GetComponent<LilGuyFollowAI>().setFollower(lilGuyAi);
            lilGuyAi.target = lilGuys.Last.Value.GetComponent<DelayedPositionUpdate>();
        }
        else
        {
            lilGuyAi.setSpeedUpDistance(lilGuysCatchUpDistance);
            lilGuyAi.target = positionUpdater;
            lilGuyAi.setHead(true);
        }
        lilGuyAi.setPlayer(this.gameObject);
        lilGuys.AddLast(spawnedObject);
        lilGuysCount++;

        return lilGuySpawn;
    }

    public void genocide()
    {
        foreach(GameObject lilGuy in lilGuys)
        {
            Destroy(lilGuy);
        }
        lilGuys.Clear();

        Debug.Log("All lilGuys killed");
    }
}
