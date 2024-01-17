using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour, IInteractable, IDamageable
{
    private bool m_holdInteract = true;
    [SerializeField] private float m_holdDuration = 2.0f;

    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Metalogic metalogic;
    [SerializeField] private ParticleSystem smoke;

    [SerializeField] AudioSource repairSound;
    [SerializeField] AudioSource damageSound;

    float smokeMax = 16;
    float smokeMin = 3;


    private float repairProgress = 0f;
    public int cost = 10;
    [Range(0f, 1f)]
    public float repairAmount = 0.1f;

    void Start(){
        cost = PersistentData.repairCost;
        m_holdDuration = PersistentData.repairTime;
    }


    public bool HoldInteract{
        get{
            return m_holdInteract;
        }
    }
    public float HoldDuration{
        get{
            return m_holdDuration;
        }
    }
    public bool isInteractable{
        get{
            return inventory.WoodCount >= cost;
        }
    }

    public bool isInvincible{
        get{
            return false;
        }
    }
    public float lifePercent{
        get {return repairProgress;}
    }

    public string HoverText{
        get{
            string text = "Repairs: ";
            text += Mathf.Floor(100f*repairProgress).ToString();
            text += "%\n";
            if (repairProgress >= 1f){
                text += "Fully Repaired!";
            }
            else if(inventory.WoodCount >= cost){
                text += "Hold 'E' to repair ("+cost.ToString()+" wood)";
            }else{
                text += "Need "+cost.ToString()+" wood to repair";
            }
            return text;
        }
    }

    public void playRepairSound(bool play){
        if(play && !repairSound.isPlaying){
            repairSound.Play();
        }
        else if (!play && repairSound.isPlaying){
            repairSound.Stop();
        }
    }
    
    public void OnInteract(){
        // repair plane
        repairProgress = Mathf.Clamp(repairProgress+repairAmount, 0, 1);
        updateSmoke();

        inventory.WoodCount -= cost;
        if(repairProgress >= 1f){
            metalogic.triggerWin();
        }
    }

    public void OnHit(float damage){
        // do damage
        repairProgress = Mathf.Clamp(repairProgress-damage*0.008f, 0, 100);
        // show alert
        metalogic.showPlaneDamageAlert();
        damageSound.Play();
        updateSmoke();
    }

    void updateSmoke(){
        var e = smoke.emission;
        e.rateOverTime = (1f-repairProgress) * (smokeMax-smokeMin) + smokeMin;
    }
}
