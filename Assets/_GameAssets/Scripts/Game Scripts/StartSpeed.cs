using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpeed : MonoBehaviour
{
    float normalSpeed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            if(PlayerPrefs.GetInt("FirstTutorial") != 0)
            {
                GameManager.instance.script_Flow.speed = GameManager.instance.realSpeed;
            }
            gameObject.SetActive(false);
        }
    }
}
