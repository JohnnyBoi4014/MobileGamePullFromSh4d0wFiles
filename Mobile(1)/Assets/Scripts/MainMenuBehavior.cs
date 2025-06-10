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

    public void SlideMenuIn(GameObject obj)
    {
        obj.SetActive(true);

        var rt = obj.GetComponent<RectTransform>();

        if (rt)
        {
            var pos = rt.position;
            pos.x = -Screen.width/2;
            rt.position = pos;

            var tween = LeanTween.moveX(rt, 0, 1.5f);
            tween.setEase(LeanTweenType.easeInOutExpo);
            tween.setIgnoreTimeScale(true);
        }
    }
    public void SlideMenuOut(GameObject obj)
    {
        obj.SetActive(true);

        var rt = obj.GetComponent<RectTransform>();

        if (rt)
        {
            var tween = LeanTween.moveX(rt, Screen.width/2, 0.5f);
            tween.setEase(LeanTweenType.easeOutQuad);
            tween.setIgnoreTimeScale(true);

            tween.setOnComplete(() => { obj.SetActive(false); });
        }
    }
}
