using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class CubeDrop : MonoBehaviour
{
    [SerializeField] private Cube cube;
    public List<GameObject> cubes = new List<GameObject>();
    

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("CubeForDrop") || other.gameObject.CompareTag("FinalObject")) && cube.dropControl && !cube.isMatched )// && other.gameObject != cube.gameObject)
        {
            cubes.Add(other.gameObject);
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false);
            GameManager.instance.CubeDropSound.Play();
            if (cube.failControl && other.gameObject.CompareTag("CubeForDrop"))
            {
                other.gameObject.GetComponent<Cube>().childCube.SetActive(false);
                other.gameObject.GetComponent<Cube>().crackCube.SetActive(true);
                cube.failControl = false;
            }
        }
    }
}
