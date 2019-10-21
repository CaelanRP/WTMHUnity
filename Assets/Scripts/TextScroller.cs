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

    public static TextScroller instance;
    void Awake(){
        instance = this;
        textMesh = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
        textMesh.text = "";
    }

    public void BloopText(string text){
        StartCoroutine(BloopTextRoutine(text));
    }
    IEnumerator BloopTextRoutine(string text){
        textMesh.text = "";
        yield return null;
        float currentIndex = 0;
        int intIndex = 0;
        textMesh.text = text;
        yield return null;
        TMPro.TMP_TextInfo textInfo = textMesh.textInfo;
        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        textMesh.maxVisibleCharacters = 0;

        var characters = text.ToCharArray();
        
        yield return null;
        Debug.Log("Showing character " + currentIndex + "/" + totalVisibleCharacters);
        while (currentIndex < totalVisibleCharacters)
        {
            Debug.Log("Showing character " + currentIndex + "/" + totalVisibleCharacters);
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

    void PlayBlip(bool final){
        if (final){
            audioSource.pitch = pitchRange.x;
        }
        else{
            audioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
        }
        audioSource.PlayOneShot(blip);
    }
}