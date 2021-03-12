using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Color color;
    public OutlineFilterObj outlineFilterObj;
    public bool isChoosen;
    public ShakeObject shakeCubes;
    public GameObject childCube;
    public string figureName;
    public Rigidbody rb_Cube;
    public bool isPressible;
    public bool dropControl;
    public GameObject crackCube;
    public bool failControl;
    public Transform tutorialHandPosition;
    public bool isFigureCube;
    public bool isMatched;

    //POWER-UPS
    public bool isSpecialCube;

    public bool bombCube;
    public Renderer bombRenderer;
    public GameObject bombModel;

    public bool colorfulCube;
    public bool catCube;


    private void Awake()
    {
        if (!isSpecialCube)
        {

            if (childCube.GetComponent<Renderer>().material.HasProperty("_Color"))
            {
                color = childCube.GetComponent<Renderer>().material.color;
            }
            else
            {
                color = childCube.GetComponent<Renderer>().material.GetColor("_Color1_F");
            }
        }
        
        isChoosen = false;
        isPressible = true;
        rb_Cube = GetComponent<Rigidbody>();
        dropControl = false;
        failControl = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && GameManager.instance.levelFailed)
        {
            StartCoroutine(CrackEffect());
        }
    }

    private IEnumerator CrackEffect()
    {
        Rigidbody[] rbs = crackCube.GetComponentsInChildren<Rigidbody>();
        int counter = 0;
        foreach(Rigidbody rb in rbs)
        {
            if (counter % 2 == 0)
            {
                rb.isKinematic = false;
            }
            else
            {
                Destroy(rb.gameObject);
            }
            counter++;
        }
        childCube.SetActive(false);
        crackCube.SetActive(true);
        failControl = true;
        yield return null;
    }
}
