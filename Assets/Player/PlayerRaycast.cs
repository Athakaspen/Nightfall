using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    private GameObject raycastedObj;
    private bool isHoldObj;
    private float holdDur;
    private float m_holdCount;

    public float holdCount {
        set{
            m_holdCount = value;
            uiProgressBar.fillAmount = holdCount/holdDur;
        }
        get{
            return m_holdCount;
        }
    }

    [SerializeField] private int rayLength = 3;
    [SerializeField] private LayerMask layerMaskInteract; // mask for trees/plane we can interact with
    [SerializeField] private LayerMask layerMaskDamage; // mask for enemies we can hit
    [SerializeField] private Animator animator;

    [SerializeField] private Image uiCrosshair;
    [SerializeField] private Text uiInteractionTip;
    [SerializeField] private Image uiProgressBar;
    [SerializeField] private float axeDamage = 4f;

    [SerializeField] private AudioSource axeSwingSound;



    void Update(){
        // stop if paused
        if (PauseMenu.gameIsPaused) return;
        if (GetComponent<PlayerHealth>().isDead) return;

        // attack when we hit LMB
        if(Input.GetButtonDown("Fire1")){
            animator.SetTrigger("doSwing");
            //axeSwingSound.Play();
        }

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, rayLength, layerMaskInteract.value)){
            
            // update lots of info
            CrosshairActive();
            raycastedObj = hit.collider.gameObject;
            IInteractable objInteractable = raycastedObj.GetComponent<IInteractable>();
            isHoldObj = objInteractable.HoldInteract;
            holdDur = objInteractable.HoldDuration;
            uiInteractionTip.text = objInteractable.HoverText;
            

            if (hit.collider.gameObject != raycastedObj){
                // new object
                holdCount = 0f;
            }

            if (raycastedObj != null && objInteractable.isInteractable){
                if (isHoldObj){
                    // code for hold-interact objects
                    if(Input.GetKey("e")){
                        holdCount += Time.deltaTime;
                        raycastedObj.GetComponent<Airplane>().playRepairSound(true);
                        //uiProgressBar.fillAmount = holdCount/holdDur;
                    } else {
                        holdCount = 0f;
                        raycastedObj.GetComponent<Airplane>().playRepairSound(false);
                        //uiProgressBar.fillAmount = holdCount/holdDur;
                    }
                    if (holdCount >= holdDur){
                        objInteractable.OnInteract();
                        uiInteractionTip.text = objInteractable.HoverText;
                        holdCount = 0f;
                    }
                } else {
                    // code for tap-interact objects
                    if(Input.GetKeyDown("e")){
                        objInteractable.OnInteract();
                    }
                }
            }
        } 
        else if (Physics.Raycast(ray, out hit, rayLength, layerMaskDamage.value)){
            
            // updates
            raycastedObj = hit.collider.gameObject;
            IDamageable objDamageable = raycastedObj.GetComponent<IDamageable>();
            
            uiProgressBar.fillAmount = objDamageable.lifePercent;
            if (objDamageable.lifePercent > 0){
                CrosshairActive();
            } else {
                CrosshairNormal();
            }

        }
        else{
            CrosshairNormal();
            uiInteractionTip.text = "";
            raycastedObj = null;
            uiProgressBar.fillAmount = 0;
        }
    }

    void CrosshairActive(){
        uiCrosshair.color = Color.red;
    }

    void CrosshairNormal(){
        uiCrosshair.color = Color.white;
    }

    public void AxeSwing(){
        axeSwingSound.Play();
    }

    public void AxeHit(){
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, rayLength, layerMaskDamage.value)){
            
            // updates
            raycastedObj = hit.collider.gameObject;
            IDamageable objDamageable = raycastedObj.GetComponent<IDamageable>();

            objDamageable.OnHit(axeDamage);
        }
    }

    public void AxeSwingFinished(){
        //Debug.Log("AxeSwingFinished");
    }

}
