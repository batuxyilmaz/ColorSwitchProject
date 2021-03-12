using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level", fileName = "Level")]
public class Level : ScriptableObject
{
    public int rowCount;
    public int columnCount;

    public bool isSpecialLevel;
    public GameObject level;

    public Material[] levelMaterials;

    public int rowFar;
}
