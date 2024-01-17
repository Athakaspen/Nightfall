using UnityEngine;

public interface IInteractable
{

    bool HoldInteract{get;}
    float HoldDuration{get;}
    bool isInteractable{get;}
    

    string HoverText{get;}
    
    void OnInteract();
}
