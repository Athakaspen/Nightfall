using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, IDamageable
{
    // private bool m_holdInteract = false;
    // private float m_holdDuration = 2.0f;
    // private string m_hoverText = "Press 'E' to collect";

    public bool titleMode = false;

    public AudioSource source;
    public AudioClip[] clips;

    private InventoryManager inventory;
    [SerializeField] private float baseWoodValue = 1;
    [SerializeField] private float max_hp = 10;
    private float hp;
    

    void Start(){
        max_hp = max_hp * this.gameObject.transform.localScale.x;
        hp = max_hp;
        if (!titleMode)
            inventory = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    public bool isInvincible{
        get{ return false;}
    }
    public float lifePercent{
        get{ return hp/max_hp;}
    }

    public void OnHit(float damage){
        hp -= damage;
        
        // play sound, adjust pitch by size
        source.pitch = 0.95f - 0.2f * this.gameObject.transform.localScale.x;
        playSound();

        if (lifePercent <= 0.02f){
            hp = 0;
            inventory.WoodCount += Mathf.RoundToInt(baseWoodValue*(this.gameObject.transform.localScale.x*1.2f));
            Destroy(this.gameObject, 0.15f);
        }
    }

    public void playSound(){
        source.clip = clips[Random.Range(0,clips.Length)];
        source.Play();
        source.volume = 0.4f + Random.Range(-.1f, .1f);
    }

}
