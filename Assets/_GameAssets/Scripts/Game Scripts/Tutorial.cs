using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // [SerializeField] private GameObject tutorialHand;

    [SerializeField] private Cube script_firstCubeForFirstTutorial;
    [SerializeField] private Cube script_secondCubeForFirstTutorial;

    private Cube script_firstCubeForSecondTutorial;
    private Cube script_secondCubeForSecondTutorial;

    private bool controlForFirstTutorial;
    private float normalSpeed;

    private bool controlForSecondTutorial;

    private void Start()
    {
        // tutorialHand = GameManager.instance.tutorialHand;
    }

    private void Update()
    {
        if (controlForFirstTutorial && script_firstCubeForFirstTutorial.isChoosen)
        {
            GameManager.instance.tutorialHand.transform.position = script_secondCubeForFirstTutorial.childCube.transform.position;
            if (script_secondCubeForFirstTutorial.isChoosen)
            {
                GameManager.instance.tutorialHand.SetActive(false);
                controlForFirstTutorial = false;

                PlayerPrefs.SetInt("FirstTutorial", 1);
                GameManager.instance.script_Flow.speed = normalSpeed;
                enabled = false;
            }
        }

        if (controlForSecondTutorial && script_secondCubeForSecondTutorial.isChoosen)
        {
            GameManager.instance.tutorialHand.SetActive(false);
            controlForSecondTutorial = false;

            PlayerPrefs.SetInt("SecondTutorial", 1);
            enabled = false;
        }
    }

    public void FirstTutorial()
    {
        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            if (cube != script_firstCubeForFirstTutorial)
            {
                cube.isPressible = false;
            }
        }

        normalSpeed = GameManager.instance.script_Flow.speed;
        GameManager.instance.script_Flow.speed = 0;

        //yukarı çıkma kodu


        GameManager.instance.tutorialHand.transform.position = script_firstCubeForFirstTutorial.childCube.transform.position;
        controlForFirstTutorial = true;
    }

    public void SecondTutorial(Cube firstCube)
    {
        script_firstCubeForSecondTutorial = firstCube;
        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            if (cube.color == firstCube.color && cube.figureName == firstCube.figureName)
            {
                script_secondCubeForSecondTutorial = cube;
                break;
            }
        }

        GameManager.instance.tutorialHand.transform.position = script_secondCubeForSecondTutorial.childCube.transform.position;
        GameManager.instance.tutorialHand.SetActive(true);
    }
}
