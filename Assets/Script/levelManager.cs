using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int _level = 0;

    private bool stopUpdating = false;

    private float _currentTime = 0;

    private float _totalTime = 0;

    private bool _timerActive = false;

    [SerializeField] private TMP_Text levelCounter;
    [SerializeField] private TMP_Text timer;
   [SerializeField] private TMP_Text lilGuyCount;


    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        GlobalReferences.LEVELMANAGER = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //reset globals

        GlobalEvents.PlayerPause.uninvoke();
        GlobalEvents.PlayerDeath.uninvoke();
        GlobalEvents.LevelComplete.uninvoke();
        GlobalEvents.PlayerStartedMoving.uninvoke();

        pauseMenu.SetActive(false);
        pauseMenu = Instantiate(pauseMenu);

        //load arena
        setLevel(1);
        //load tutorial

    }

    // Update is called once per frame
    void Update()
    {
        if (stopUpdating) return;

        if (GlobalEvents.PlayerStartedMoving.Invoked() && !GlobalEvents.PlayerPause.Invoked())
        {
           _timerActive = true;
        }

        if (GlobalEvents.PlayerDeath.Invoked())
        {
            restartLevel();
            GlobalEvents.PlayerDeath.uninvoke();
            GlobalEvents.LevelComplete.uninvoke();
            return;
        }

        if (GlobalEvents.LevelComplete.Invoked())
        {
            loadMainMenu();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && !GlobalEvents.PlayerPause.Invoked())
        {
            if (GlobalEvents.PlayerPause.Invoked()) togglePauseMenu();
            GlobalEvents.PlayerPause.uninvoke();
            restartLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GlobalEvents.PlayerPause.Invoked()) togglePauseMenu();
            GlobalEvents.PlayerPause.uninvoke();
            GlobalEvents.PlayerStartedMoving.uninvoke();
            GlobalEvents.FullPlaythroughInProgress.uninvoke();
            loadMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            //pause button
            togglePauseMenu();

        }

        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        timer.text = time.Minutes + ":" + time.Seconds + ":" + time.Milliseconds;

        lilGuyCount.text = ": " + GlobalReferences.PLAYER.LilGuyCount + "x";




    }

    public void setLevel(int level)
    {

        if (level == 0)
        {
            Debug.Log("Some bozo tried to set the level to 0");
            throw new Exception("level cannot be set to 0");
        }

        Unloadlevel(() =>
        {

            //TODO: uncomment code after making levels
            // Debug.Log("Loading level: " + this._level);
            // SceneManager.LoadSceneAsync("level " + this._level, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
            // {

            //      Debug.Log("Loaded level: " + this._level);

            //      levelCounter.text = "Level " + this._level;
            //};
            loadPlayerMovement();

        });
    }

    public void Unloadlevel(System.Action callback)
    {
        Debug.Log("unload level called for level: " + this._level);
        if (this._level > 0)
        {
            Debug.Log("Unloading level: " + this._level);
            SceneManager.UnloadSceneAsync("level " + this._level).completed += (asyncOperation) =>
            {
               callback();
            };
        } else
        {
            callback();
        }
    }

    public void incrementLevel() { setLevel(this._level + 1); }

    public void restartLevel()
    {
        setLevel(this._level);
    }

    public void loadMainMenu()
    {
        Unloadlevel(() => 
        { 
            SceneManager.LoadSceneAsync("MainMenu", mode: LoadSceneMode.Additive).completed+=(asyncOperation)=> 
            { 
                SceneManager.UnloadSceneAsync("LevelController");
            };
        });
    }

    private void togglePauseMenu()
    {
        Debug.Log("Toggling pause");
        if (GlobalEvents.PlayerPause.Invoked())
        {
            //SceneManager.UnloadSceneAsync("PauseMenu").completed += (asyncOperation) => 
           // {
                GlobalEvents.PlayerPause.uninvoke();
            //};

            if (GlobalEvents.PlayerStartedMoving.Invoked()) _timerActive = false;

            pauseMenu.SetActive(false);
        }
        else
        {
          //  SceneManager.LoadSceneAsync("PauseMenu", mode: LoadSceneMode.Additive).completed += (asyncOperation) => 
           // { 
                GlobalEvents.PlayerPause.invoke();
            //  };

            _timerActive = false;


            pauseMenu.SetActive(true);
        }
    }

    private string levelString()
    {
        return "level" + this._level;
    }

    private void loadPlayerMovement()
    {
        SceneManager.LoadSceneAsync(SceneNames.PLAYERMOVEMENT, mode: LoadSceneMode.Additive).completed += (asyncOperation) =>
        {
            levelCounter.text = "Debug Level";
        };
    }
}
