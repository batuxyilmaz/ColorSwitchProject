using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{
    public float speed = .01f;

    private void Awake()
    {
       // speed = 0f;
        GameManager.instance.script_Flow = this;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cube") && !GameManager.instance.isFinished)
        {
            other.transform.parent.position += transform.up * speed;
        }

        if (other.CompareTag("FinalObject"))
        {
            if (GameManager.instance.levelCompleted)
            {
                if (GameManager.instance.springInFinalObject.transform.localScale.y > 0)
                {
                    GameManager.instance.springInFinalObject.transform.localScale += new Vector3(0, speed * .13f * 4f, 0);
                    other.transform.parent.GetChild(1).GetChild(0).position += transform.up * speed * 4f;
                }
            }
            else
            {
              //other.transform.parent.position += transform.up * speed;
                other.GetComponentInParent<Rigidbody>().transform.position += transform.up * speed;
            }
        }

        if (other.CompareTag("StartObject") && !GameManager.instance.isFinished)
        {
            other.transform.position += transform.up * speed;
        }
    }
}
