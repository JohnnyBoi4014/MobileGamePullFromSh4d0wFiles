using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuBehavior : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    public GameObject controlPanel; // something that's supposed to be in chapter 12
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        // check for a high score and set it to our TMProUGUI
        GetAndDisplayScore();
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

    public void ResetScore(UnityEngine.UI.Button resetButton)
    {
        //experiment meant to turn button green
        resetButton.image.color = Color.green;

        //Prof Tanner's stuff
        PlayerPrefs.SetInt("score", 0);
        GetAndDisplayScore();
    }

    private void GetAndDisplayScore()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("score").ToString();
    }
}
