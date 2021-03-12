using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWrongMatch : MonoBehaviour
{
    private bool isTutorialActive = false;

    private Cube script_firstCube;
    private Cube script_secondCube;

    private GameObject tutorialHand;
    private GameObject effectCircle;

    // Start is called before the first frame update
    void Start()
    {
        tutorialHand = GameManager.instance.tutorialHand;
        effectCircle = GameManager.instance.effectCircle;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorialActive && script_secondCube.isChoosen)
        {
            effectCircle.SetActive(false);
            UIManager.instance.panel_tutorial.SetActive(false);

            isTutorialActive = false;

            PlayerPrefs.SetInt("WrongMatchTutorial", 1);

            foreach (Cube cube in GameManager.instance.list_script_cubes)
            {
                cube.isPressible = true;
            }

            enabled = false;
        }

        if (isTutorialActive)
        {
            GameManager.instance.tutorialHand.transform.position = script_secondCube.tutorialHandPosition.position;
            GameManager.instance.effectCircle.transform.position = script_secondCube.tutorialHandPosition.position;
        }
    }

    public void WrongMatchTutorial(Cube firstCube)
    {
        script_firstCube = firstCube;
        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            if (cube.color == firstCube.color && cube.figureName == firstCube.figureName)
            {
                script_secondCube = cube;
                break;
            }
        }

        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            if (cube != script_secondCube)
            {
                cube.isPressible = false;
            }
        }

        GameManager.instance.tutorialHand.transform.position = script_secondCube.tutorialHandPosition.position;
        GameManager.instance.effectCircle.transform.position = script_secondCube.tutorialHandPosition.position;

        GameManager.instance.effectCircle.SetActive(true);
        UIManager.instance.panel_tutorial.SetActive(true);

        isTutorialActive = true;
    }
}
