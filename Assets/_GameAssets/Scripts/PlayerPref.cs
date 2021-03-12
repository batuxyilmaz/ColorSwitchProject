using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPref : MonoBehaviour
{
    public static void SetInts(string key, List<int> collection)
    {
        PlayerPrefs.SetInt(key + ".Count", collection.Count);

        for(int i = 0; i < collection.Count; i++)
        {
            PlayerPrefs.SetInt(key + "[" + i.ToString() + "]", collection[i]);
        }
    }

    public static List<int> GetInt(string key)
    {
        int count = PlayerPrefs.GetInt(key + ".Count");
        List<int> array = new List<int>();

        for(int i = 0; i < count; i++)
        {
            array.Add(PlayerPrefs.GetInt(key + "[" + i.ToString() + "]"));
        }

        return array;
    }
}
