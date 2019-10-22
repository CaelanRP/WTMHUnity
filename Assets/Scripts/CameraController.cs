using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static Camera _c;
    public static Camera mainCam{
        get{
            if (_c == null){
                _c = Camera.main;
            }
            return _c;
        }
    }
    

    public static void Disable(){
        mainCam.cullingMask = 0;
    }

    public static void Enable(){
        mainCam.cullingMask = ~0;
    }
}
