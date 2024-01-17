using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private Text text;
    public Color normalColor = new Color(1,1,1,1);
    public Color hoverColor = new Color(1,1,1,0.6f);

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
     {
         text.color = hoverColor; //Or however you do your color
     }
 
     public void OnPointerExit(PointerEventData eventData)
     {
         text.color = normalColor; //Or however you do your color
     }
}
