using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <param name="levelName">The name of the lvl we want to go to</param>
    /// 
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
