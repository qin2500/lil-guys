using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayGame(){

        SceneManager.UnloadSceneAsync(SceneNames.MAINMENU).completed += (asyncOperation) =>
        SceneManager.LoadSceneAsync(SceneNames.LEVELCONTROLLER, LoadSceneMode.Additive); // make sure the in the build, we have certain scenes preloaded
    }
    public void QuitGame(){
        Application.Quit();
    }
    
}
