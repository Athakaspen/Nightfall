using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FinalTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = GetComponent<Text>();
        text.text = "...In " + PersistentData.endDays + " Days and " + PersistentData.endNights + " Nights";
    }
}
