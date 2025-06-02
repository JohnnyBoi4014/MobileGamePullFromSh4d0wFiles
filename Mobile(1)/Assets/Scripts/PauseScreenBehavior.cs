using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehavior : MonoBehaviour
{

    public static bool paused;

    [Tooltip("Reference to the pause menu object to turn on/off")]
    public GameObject pauseMenu;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// aaa
    /// </summary>
    /// <param name="isPaused"></param>
    /// 
    public void SetPauseMenu(bool isPaused)
    {
        paused = isPaused;

        Time.timeScale = (paused) ? 0 : 1;

        pauseMenu.SetActive(paused);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        paused = false;

        // My added fix to a problem
        SetPauseMenu(paused);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
