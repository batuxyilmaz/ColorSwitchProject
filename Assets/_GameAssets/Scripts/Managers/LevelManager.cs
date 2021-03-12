using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    [Header("DEBUG")]
    public int currentLevel;
    public int levelCount;
    public Text levelText;
    public int levelToBeLoaded;

    public int levelNumberForSpeed;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        currentLevel = PlayerPrefs.GetInt("Level");
        levelNumberForSpeed = PlayerPrefs.GetInt("SpeedLevel");
        levelText.text = "LEVEL" +" "+ (currentLevel + 1);
        levelCount = GameManager.instance.levels.Length;
        // levelCount = GameManager.instance.levelTextures.Length;

        
        
        ControlLevelNumber();
    }

    public void NextLevel()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        GameObject[] previousLevels = GameObject.FindGameObjectsWithTag("Level");
        foreach(GameObject go in previousLevels)
        {
            Destroy(go);
        }
        DefaultValues();
    }

    public void ControlLevelNumber()
    {
        if (currentLevel >= levelCount)
        {
            levelToBeLoaded = Random.Range(8, levelCount);
            LoadLevel(levelToBeLoaded);
        }
        else
        {
            levelToBeLoaded = currentLevel;
            LoadLevel(levelToBeLoaded);
        }
    }

    private void DefaultValues()
    {
        if (currentLevel < 6)
        {
            levelNumberForSpeed = 0;
        }
        else if (levelNumberForSpeed % 4 == 0)
        {
            levelNumberForSpeed++;
        }

        PlayerPrefs.SetInt("SpeedLevel", levelNumberForSpeed);
        GameManager.instance.DefaultValues();
    }
}
