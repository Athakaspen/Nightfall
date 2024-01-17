using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private float max_hp = 30;
    private float hp;
    public bool isDead = false;
    [SerializeField] private float regenDelay = 10f;
    [SerializeField] private float regenRate = 0.4f;
    private float timeSinceLastHit;
    [SerializeField] private Image uiHealthMask;
    float uiHealthMaskMax;
    [SerializeField] private Gradient healthColor;
    [SerializeField] private FadeToBlack uiFade;
    [SerializeField] private Metalogic metalogic;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioClip oof;
    [SerializeField] AudioClip ooof;

    private bool m_isInvincible = false;


    public bool isInvincible{
        get{
            return m_isInvincible;
        }
        set{
            m_isInvincible = value;
        }
    }
    public float lifePercent{
        get{ return hp/max_hp;}
    }

    void Start(){
        hp = max_hp;
        uiHealthMaskMax = uiHealthMask.GetComponent<RectTransform>().rect.width;
        updateUI();
    }

    public void OnHit(float damage){
        if (isInvincible) return;
        if (isDead) return;
        hp = Mathf.Clamp(hp-damage, 0, max_hp);
        timeSinceLastHit = 0;
        // reset our current action (eg repairing plane)
        this.gameObject.GetComponent<PlayerRaycast>().holdCount = 0;
        Debug.Log("hit for "+damage.ToString()+" damage!");
        updateUI();
        if (hp <= 0){
            doDie();
        } else {
            hitSound.clip = oof;
            hitSound.Play();
        }
    }
    void doDie(){
        hitSound.clip = ooof;
        hitSound.Play();
        if (isDead) return;
        isDead = true;
        metalogic.doPlayerDeath();
    }

    public void reset(){
        isDead = false;
        hp = max_hp;
        updateUI();
    }

    void Update(){
        timeSinceLastHit += Time.deltaTime;
        if (hp < max_hp && timeSinceLastHit > regenDelay){
            hp = Mathf.Clamp(hp + regenRate * Time.deltaTime, 0, max_hp);
            updateUI();
        }
    }

    void updateUI(){
        uiHealthMask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lifePercent * uiHealthMaskMax);
        uiHealthMask.transform.GetChild(0).GetComponent<Image>().color = healthColor.Evaluate(lifePercent);
    }
}
