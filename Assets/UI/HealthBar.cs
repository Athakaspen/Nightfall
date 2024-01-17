using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    float health = 0.5f;
    public Texture2D innerTex;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setHealth(float value){
        health = value;
    }

    void OnGUI()
    {

    }
}
