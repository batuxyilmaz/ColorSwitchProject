using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LevelNumber;
    public GameObject StartPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject awardPanel;

    public GameObject panel_tutorial;

    public GameObject bar1;
    
    public int currentLevel;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ClosePanels();
        StartPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        ClosePanels();
        LevelNumber.SetActive(true);
        Time.timeScale = 1f;
        GameManager.instance.canTouch = true;
    }

    public void ShowLevelFailedPanel()
    {
        ClosePanels();
        LevelNumber.SetActive(true);
        levelFailedPanel.SetActive(true);
    }

    public void ShowLevelCompletedPanel()
    {
        ClosePanels();
        LevelNumber.SetActive(true);
        levelCompletedPanel.SetActive(true);
    }

    public void HomeButton()
    {
        ClosePanels();
        awardPanel.SetActive(false);
        AwardManager.instance.popUp.SetActive(false);
        StartPanel.SetActive(true);
        bar1.SetActive(true);
       
        LevelManager.instance.ControlLevelNumber();
        Time.timeScale = 0f;
       
    }
    public void ShowAwardPanel()
    {
        GameManager.instance.canTouch = false;
        ClosePanels();
        awardPanel.SetActive(true);
        bar1.SetActive(true);
      
        GameManager.instance.totalScore = PlayerPrefs.GetInt("Score");
        PlayerPrefs.SetInt("Score", GameManager.instance.totalScore);
        LevelNumber.SetActive(false);

        if (GameManager.instance.totalScore >= 10)
       {
            AwardManager.instance.buyButton.GetComponent<Image>().color = Color.white;
            AwardManager.instance.buyButton.interactable = true;
        }
        else
        {
            AwardManager.instance.buyButton.GetComponent<Image>().color = new Color(212f, 213f, 213f, 181f);
            AwardManager.instance.buyButton.interactable = false;
        }
        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            cube.childCube.layer = LayerMask.NameToLayer("Cube");
        }

    }

    public void ClosePanels()
    {
        StartPanel.SetActive(false);
        levelCompletedPanel.SetActive(false);
        levelFailedPanel.SetActive(false);
        awardPanel.SetActive(false);
        panel_tutorial.SetActive(false);

        // LevelNumber.SetActive(false);
    }

    public void LevelAdding()
    {
        currentLevel = LevelManager.instance.currentLevel;
        currentLevel = PlayerPrefs.GetInt("Level");
        LevelManager.instance.levelText.text = "LEVEL" + " " + (currentLevel + 1);
        PlayerPrefs.SetInt("Level", currentLevel);
    }
    public void PointControl(Text lastScore,int totalScore)
    {
        
        int deger = 0;
       
        lastScore = GameManager.instance.lastScore;
        totalScore = GameManager.instance.totalScore;
      
        
            if (totalScore % 1000 == 0)
            {
                deger++;
                lastScore.text = deger + "K";
            }
         
    }

    
}
