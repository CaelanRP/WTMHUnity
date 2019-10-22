using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedThing : MonoBehaviour
{
    public string textureName;

    public int startIndex, endIndex, defaultIndex;
    public float speed;

    public bool startOnAwake;

    float currentIndex;

    private Sprite[] _S;
    public Sprite[] sprites{
        get{
            if (_S == null){
                _S = Resources.LoadAll<Sprite>(textureName);
            }
            return _S;
        }
    }

    protected SpriteRenderer spr;

    void Awake(){
        spr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start(){
        if (startOnAwake){
            StartAnimation();
        }
    }

    IEnumerator Animate(){
        int intIndex = startIndex;
        currentIndex = startIndex;
        while(true){
            currentIndex += Time.deltaTime * speed;
            int testInt = Mathf.FloorToInt(currentIndex);
            if (testInt > intIndex){
                intIndex = testInt;
                if (intIndex > endIndex){
                    intIndex = startIndex;
                    currentIndex = startIndex;
                }
                OnFrameAdvance();
            }
            if (stopped){
                stopped = false;
                yield break;
            }
            spr.sprite = sprites[intIndex];
            yield return null;
        }
    }

    void OnFrameAdvance(){
        if (stopAfterFrame){
            StopAnimation();
            stopAfterFrame = false;
        }
    }

    public void StartAnimation(){
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    public void StopAnimation(){
        stopped = true;
        spr.sprite = sprites[defaultIndex];
    }

    public void StopAfterFrame(){
        stopAfterFrame = true;
    }

    bool stopAfterFrame;
    bool stopped;

    public void SetAnimation(int startIndex, int endIndex, float speed){
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.speed = speed;
    }
}
