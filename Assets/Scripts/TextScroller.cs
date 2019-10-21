using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
public class TextScroller : MonoBehaviour
{
    public int charsPerSecond;

    TextMeshPro textMesh;

    AudioSource audioSource;
    public AudioClip blip;
    [MinMaxSlider(0,2)]
    public Vector2 pitchRange;
    public float blipRange;

    public static TextScroller instance;
    void Awake(){
        instance = this;
        textMesh = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
        textMesh.text = "";
    }

    public void BloopText(string text){
        targetPitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
        StartCoroutine(BloopTextRoutine(text));
    }
    IEnumerator BloopTextRoutine(string text){
        textMesh.text = "";
        yield return null;
        float currentIndex = 0;
        int intIndex = 0;
        textMesh.maxVisibleCharacters = 0;
        textMesh.text = text;
        TMPro.TMP_TextInfo textInfo = textMesh.textInfo;
        yield return null;
        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object

        var characters = text.ToCharArray();
        
        yield return null;

        textMesh.enabled = true;
        while (currentIndex < totalVisibleCharacters)
        {
            currentIndex = Mathf.Min (currentIndex + (charsPerSecond * Time.deltaTime), totalVisibleCharacters);
            var i = Mathf.FloorToInt(currentIndex);
            if (i > intIndex && i > 0){
                intIndex = i;
                char currentChar = characters[i-1];
                if (!char.IsWhiteSpace(currentChar)){
                    PlayBlip(i == characters.Length);
                }
            }
            textMesh.maxVisibleCharacters = intIndex;
            yield return null;
        }
    }

    public string testString;

    [Button("Test Text")]
    void TestText(){
        BloopText(testString);
    }

    float targetPitch;

    void PlayBlip(bool final){
        if (final){
            audioSource.pitch = targetPitch - blipRange;
        }
        else{
            audioSource.pitch = UnityEngine.Random.Range(targetPitch - blipRange, targetPitch + blipRange);
        }
        audioSource.PlayOneShot(blip);
    }
}