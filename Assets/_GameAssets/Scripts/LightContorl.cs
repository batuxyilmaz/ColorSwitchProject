using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightContorl : MonoBehaviour
{
    public Animation lightAnimation;
    public static LightContorl instance;

    public void Awake()
    {
        instance = this;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Cube")
        {
            lightAnimation.Play("LightFading");
        }
    }
   
}
