using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{

    public Gradient fadeGrade;
    public Image img;

    void Start(){
        DoFade(false, 0.6f);
    }

    public void DoFade(bool fadetoBlack = true, float duration = 2f){
        //Debug.Log(duration);
        StartCoroutine(FadeRoutine(fadetoBlack, duration));
    }

    IEnumerator FadeRoutine(bool fadeToBlack, float dur){
        // fade from transparent to opaque
        if (fadeToBlack)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime/dur)
            {
                // set color with i as alpha
                img.color = fadeGrade.Evaluate(i);
                yield return null;
            }
            img.color = fadeGrade.Evaluate(0f);
        }
        // fade from opaque to transparent
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime/dur)
            {
                // set color with i as alpha
                img.color = fadeGrade.Evaluate(i);
                yield return null;
            }
            img.color = fadeGrade.Evaluate(1f);
        }
    }

}
