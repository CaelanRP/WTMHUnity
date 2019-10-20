using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float textDelay;
    void Start()
    {
        StartCoroutine(EnterHouseRoutine());
    }

    IEnumerator EnterHouseRoutine(){
        HouseGen.instance.GenerateHouse();
        yield return new WaitForSeconds(textDelay);
        TextScroller.instance.BloopText("WELCOME TO MY HOUSE");
    }
}
