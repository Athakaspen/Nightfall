using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FinalDifficulty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = GetComponent<Text>();
        text.text = "(" + PersistentData.difficulty + ")";
        if (PersistentData.difficulty == "Lunatic")
            text.color = Color.red;
    }
}
