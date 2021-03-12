using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelFailed : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube" && !GameManager.instance.levelFailed)
        {
            DOTween.To(() => GameManager.instance.script_Flow.speed, (x) => GameManager.instance.script_Flow.speed = x, .14f, .5f);
            GameManager.instance.LevelFailed();
            print("levelfailed");
            //   gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("FinalObjectForFail"))
        {
            DOTween.KillAll();
            GameManager.instance.script_Flow.speed = 0;
            UIManager.instance.ShowLevelFailedPanel();
            GameManager.instance.CubeDropSound.mute = true;
        }

    }
}
