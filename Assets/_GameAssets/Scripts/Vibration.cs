using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class Vibration : MonoBehaviour
{
 public void VibrationAdd()
    {
        print("titrettim");
        MMVibrationManager.Haptic(HapticTypes.Failure);
    }




}

