using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private int max_wood = 25;
    private int m_woodcount = 0;

    public int WoodCount{
        get{
            return m_woodcount;
        }
        set{
            m_woodcount = Mathf.Clamp(value, 0, max_wood);
            UpdateUI();
        }
    }

    [SerializeField]
    private Text uiWoodCount;
    [SerializeField]
    private Text uiWoodLabel;

    void Start(){
        max_wood = PersistentData.maxWood;
        UpdateUI();
    }


    void UpdateUI()
    {
        // update UI
        uiWoodCount.text = WoodCount.ToString() + "/" + max_wood;
        if (WoodCount==max_wood){
            uiWoodCount.color = Color.red;
            uiWoodLabel.color = Color.red;
        }
        else {
            uiWoodCount.color = Color.white;
            uiWoodLabel.color = Color.white;
        }
    }
}
