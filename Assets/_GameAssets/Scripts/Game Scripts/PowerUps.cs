using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public IEnumerator GoBack()
    {
        GameManager.instance.script_Flow.speed = GameManager.instance.script_Flow.speed * -2f;
        yield return new WaitForSeconds(1f);
        GameManager.instance.script_Flow.speed = GameManager.instance.script_Flow.speed / -2f;
    }

    public IEnumerator BombSameColorsCube(List<Cube> cubes, Color color, Cube bombCube1, Cube bombCube2)
    {
        List<Cube> destroyCubes = new List<Cube>();
        
        foreach (Cube cube in cubes)
        {
            
            if (cube.color == color && cube != bombCube1 && cube != bombCube2)
            {
                destroyCubes.Add(cube);
            }
        }
        

        int i = 0;
        foreach (Cube cube in destroyCubes)
        {
            if (cube)
            {
                GameManager.instance.DestroySameColorsCube(cube, bombCube1, bombCube2, i % 2);
                i++;
                yield return new WaitForSeconds(.0f);
            }
        }
    }

    public IEnumerator CatSpargePaws(List<Cube> cubes, Cube catCube1, Cube catCube2)
    {
        int i = 0;

        List<Cube> allCubes = new List<Cube>();

        foreach(Cube cube in cubes)
        {
            allCubes.Add(cube);
        }
        
        foreach (Cube cube in allCubes)
        {
            if (cube != catCube1 && cube != catCube2)
            {
                GameManager.instance.SpargePaws(cube, catCube1, catCube2, i % 2);
                i++;
                yield return new WaitForSeconds(.0f);
            }
        }
    }
}
