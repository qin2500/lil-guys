using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LilGuysManager : MonoBehaviour
{
    [SerializeField] private GameObject lilGuyPrefab;
    [SerializeField] private GameObject lilGuySpawn;
    [SerializeField] private DelayedPositionUpdate positionUpdater;
    [SerializeField] private float lilGuysCatchUpDistance = 3f;//Distance when lil guys start to speed up to catch up to you.
    [SerializeField] private GameObject homicideParticleEffect;
    [SerializeField] private float throwPower;
    [SerializeField] private float timeToDisableThrownGuy;



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
        if (lilGuys.Count > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                bonusJump();
            }
            else if (Input.GetMouseButtonDown(0)) 
            {
                throwGuy();
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            summonBoy();
        }

        GlobalReferences.PLAYER.LilGuyCount = lilGuys.Count;
    }

    public GameObject summonBoy()
    {
        GameObject spawnedObject = Instantiate(lilGuyPrefab, lilGuySpawn.transform.position, Quaternion.identity);        
        spawnedObject.SetActive(true);

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

    private void bonusJump()
    {
        homicide(expend());
        playerMovement.bonusJump();
    }

    private void throwGuy()
    {
        GameObject lilGuy = expend();

        //throw guy
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - GlobalReferences.PLAYER.PlayerObject.transform.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        lilGuy.transform.position = GlobalReferences.PLAYER.PlayerObject.transform.position;
        lilGuy.transform.rotation = rotation;

        Throwable throwable = lilGuy.GetComponent<Throwable>();
        throwable.triggerThrow(throwPower, timeToDisableThrownGuy);

        //pass particle effect - TODO: replace with destroyable class and handle particle effects there
        throwable.collisionParticleEffect = homicideParticleEffect;
    }

    private GameObject expend()
    {
        Debug.Log("expending guy - whoever receives it has to handle destruction");

        //pick guy
        GameObject lilGuy = lilGuys.Last.Value;

        //disable his ai
        lilGuy.GetComponent<LilGuyFollowAI>().giveJesusTheWheel();

        //remove guy
        lilGuys.Remove(lilGuy);

        return lilGuy;
    }

    private void homicide(GameObject lilGuy)
    {
        Debug.Log("homicided a guy");
        //particle effect
        Instantiate(homicideParticleEffect, lilGuy.transform.position, Quaternion.identity);

        //murder
        Destroy(lilGuy);

        lilGuys.Remove(lilGuy);
    }


}
