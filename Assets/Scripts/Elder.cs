using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elder : AnimatedThing
{
    protected override void Start(){
        
    }

    public void RandomizeElder(){
        startIndex = (Random.Range(0,(sprites.Length/2)) * 2);
        endIndex = startIndex + 1;
        defaultIndex = startIndex;
        spr.sprite = sprites[defaultIndex];
        base.Start();
    }
}
