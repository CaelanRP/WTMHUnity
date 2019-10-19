using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
public class TextScroller : MonoBehaviour
{
    public int charsPerSecond;

    TextMeshPro textMesh;

    void Awake(){
        textMesh = GetComponentInChildren<TextMeshPro>();
    }
    IEnumerator BloopText(string text){
        TMPro.TMP_TextInfo textInfo = textMesh.textInfo;
        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        float currentIndex = 0;
        int intIndex = 0;
        textMesh.text = text;
        textMesh.maxVisibleCharacters = 0;
        yield return null;
        while (currentIndex < totalVisibleCharacters)
        {
            currentIndex = Mathf.Min (currentIndex + (charsPerSecond * Time.deltaTime), totalVisibleCharacters);
            var i = Mathf.FloorToInt(currentIndex);
            if (i > intIndex){
                // TODO audio bloops
                intIndex = i;
            }
            textMesh.maxVisibleCharacters = intIndex;
            yield return null;
        }
    }

    public string testString;

    [Button("Test Text")]
    void TestText(){
        StartCoroutine(BloopText(testString));
    }


}