using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
        Debug.Log("starting game");

        SceneManager.LoadSceneAsync("MainMenu", mode: LoadSceneMode.Additive);
    }
}
