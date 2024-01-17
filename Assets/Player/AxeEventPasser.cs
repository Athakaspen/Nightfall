using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeEventPasser : MonoBehaviour
{

    [SerializeField] private PlayerRaycast destination;
    
    void AxeHit(){
        destination.AxeHit();
    }

    public void AxeSwingFinished(){
        destination.AxeSwingFinished();
    }

    void AxeSwing(){
        destination.AxeSwing();
    }
}
