using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialHowToPlay : MonoBehaviour
{
    private GameObject handTutorial;

    [SerializeField] private Cube script_firstCube;
    [SerializeField] private Cube script_secondCube;

    private float normalSpeed;
    private bool isTutorialActive = false;

    [SerializeField] private Transform levelStopPoint;

    private void Awake()
    {
        GameManager.instance.script_tutorialHowToPlay = this;
        handTutorial = GameManager.instance.tutorialHand;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorialActive && script_firstCube.isChoosen)
        {
            GameManager.instance.tutorialHand.transform.position = script_secondCube.tutorialHandPosition.position;
            GameManager.instance.effectCircle.transform.position = script_secondCube.tutorialHandPosition.position;

            script_secondCube.isPressible = true;

            if (script_secondCube.isChoosen)
            {
                GameManager.instance.effectCircle.SetActive(false);
                UIManager.instance.panel_tutorial.SetActive(false);

                isTutorialActive = false;

                PlayerPrefs.SetInt("FirstTutorial", 1);
                GameManager.instance.script_Flow.speed = normalSpeed;

                foreach(Cube cube in GameManager.instance.list_script_cubes)
                {
                    if (cube)
                    {
                        cube.isPressible = true;
                    }
                }

                enabled = false;
            }
        }
    }

    public void HowToPlayTutorial()
    {
        foreach (Cube cube in GameManager.instance.list_script_cubes)
        {
            if (cube != script_firstCube)
            {
                cube.isPressible = false;
            }
        }

        normalSpeed = GameManager.instance.realSpeed;
        GameManager.instance.script_Flow.speed = 0;

        //yukarı çıkma kodu
        transform.DOMoveY(levelStopPoint.position.y, 1f).OnComplete(() =>
        {
            GameManager.instance.tutorialHand.transform.position = script_firstCube.tutorialHandPosition.position;
            GameManager.instance.effectCircle.transform.position = script_firstCube.tutorialHandPosition.position;

            GameManager.instance.effectCircle.SetActive(true);
            UIManager.instance.panel_tutorial.SetActive(true);

            isTutorialActive = true;
        });
    }
}
