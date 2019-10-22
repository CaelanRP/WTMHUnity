using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float textDelay;
    AudioSource audioSource;

    public float musicFadeTime;
    public List<AudioClip> houseMusic;

    private Grammar _G;
    public Grammar grammar{
        get{
            if (_G == null){
                _G = new Grammar("wtmh_grammar", "wtmh_vocab");
            }
            return _G;
        }
    }

    public static GameManager instance;

    void Awake(){
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(EnterHouseRoutine());
    }

    void Update(){

    }

    void Reset(){
        StopAllCoroutines();
        TextScroller.instance.StopAllCoroutines();
        HouseGen.instance.StopAllCoroutines();
    }

    [HideInInspector]
    public bool transitioning;

    IEnumerator EnterHouseRoutine(){
        transitioning = true;
        HouseGen.instance.GenerateHouse();
        PlayMusic();
        yield return Player.instance.WalkIntoHouse();
        yield return new WaitForSeconds(textDelay);
        yield return TextScroller.instance.BloopTextRoutine(grammar.Sample("main"));
        transitioning = false;
        //TextScroller.instance.BloopText("WELCOME TO MY HOUSE");
    }

    IEnumerator ReloadHouseRoutine(){
        Reset();
        yield return StartCoroutine(EnterHouseRoutine());
    }

    public void ExitHouse(){
        StopMusic();
        StartCoroutine(ReloadHouseRoutine());
    }

    AudioClip prevMusic = null;
    void PlayMusic(){
        audioSource.Stop();
        audioSource.clip = houseMusic.Where(c => c != prevMusic).RandomSelection();
        prevMusic = audioSource.clip;
        StartCoroutine(FadeInMusic(musicFadeTime));
    }

    void StopMusic(){
        audioSource.Stop();
    }

    IEnumerator FadeInMusic(float FadeTime) {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1) {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
