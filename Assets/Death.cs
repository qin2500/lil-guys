using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update

    int frames = 0;
    System.Action callback;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (frames == 0) return;


        if (frames > 1) { frames--; return; }

        //then start level back
        Debug.Log("death calling callback");
        callback(); //frames has to be less than particle time if this is attached to a particle
        frames--;
    }

    public void die(int frames, System.Action callback)
    {
        this.frames = frames;
        this.callback = callback;
    }
}
