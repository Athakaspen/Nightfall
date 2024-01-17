using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour, IDamageable
{
    private GameObject player;
    private GameObject plane;
    private GameObject target;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackDamage = 3f; // seconds
    [SerializeField] private float attackCooldown = 1f; // seconds
    [SerializeField] private float max_hp = 20;
    private float hp;

    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioSource chatterSound;
    float timeTilChatter = 1f;
    [SerializeField] AudioSource screechSound;
    [SerializeField] AudioClip[] screeches;
    [SerializeField] AudioClip deathScreech;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource attackSound;

    private float approachMargin; // how close we have to be to attack
    private float attackTimer;
    private bool isAttacking; // are we in the attack animation
    public bool isDead; // are we ded
    private bool isRecoiling; // are we in the hit animation
    private bool isActive{ // this tells us if we should update our target, move, etc.
        get{
            return !isAttacking && !isDead && !isRecoiling;
        }
    }
    private bool isSinking;

    public float moveSpeed = 12f;

    public float gravity = -9.8f;

    public bool isInvincible{
        get{ return false;}
    }
    public float lifePercent{
        get { return hp/max_hp;}
    }

    Vector3 velocity;

    void Start(){
        animator.SetBool("isIdle", true);
        hp = max_hp;
        player = GameObject.FindGameObjectWithTag("Player");
        plane = GameObject.FindGameObjectWithTag("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.gameIsPaused)
            walkSound.Stop();
        else
            walkSound.Play();
        // if (Input.GetKeyDown("k")){
        //     OnHit(4);
        // }
        if (isActive){
            // get nearest target
            Vector3 toPlayer = player.transform.position - this.gameObject.transform.position;
            Vector3 toPlane = plane.transform.position - this.gameObject.transform.position;
            Vector3 toTarget;
            

            if (toPlane.magnitude < toPlayer.magnitude && plane.GetComponent<IDamageable>().lifePercent > 0){
                // Plane is closer, so that's our target
                target = plane;
                toTarget = toPlane;
                approachMargin = 6.6f;
            } else {
                // Player is closer
                target = player;
                toTarget = toPlayer;
                approachMargin = 3.4f;
            }

            // look at the target
            transform.LookAt(target.transform, Vector3.up);
            // Zeroe out up/down rotation
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

            // check distance to determine action to take
            if(toTarget.magnitude > approachMargin){
                // approach target
                controller.Move(toTarget.normalized* moveSpeed * Time.deltaTime);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                walkSound.volume = 1f;
                timeTilChatter -= Time.deltaTime;
                if (timeTilChatter <= 0f){
                    chatterSound.Play();
                    timeTilChatter = Random.Range(2f, 4f);
                }
            } else {
                // in range to attack
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                walkSound.volume = 0f;
                if (attackTimer <= 0){
                    // do attack & reset cooldown
                    animator.SetTrigger("doAttack");
                    attackTimer = attackCooldown;
                    isAttacking = true; // this sets us as inactive so we wait for the animation to finish
                } else {
                    // just update cooldown
                    attackTimer -= Time.deltaTime;
                }
            }
        }

        if (!isDead){
            // gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        } else if (isSinking){
            transform.position += Vector3.down * 0.3f * Time.deltaTime;
            if (transform.position.y <= 0) Destroy(this.gameObject);
        }
    }

    public void fallThroughGround(){
        isSinking = true;
    }

    void Attack(){
        if ((target.transform.position - this.gameObject.transform.position).magnitude <= approachMargin*1.3f
                && isAttacking
                && !target.GetComponent<IDamageable>().isInvincible){
            target.GetComponent<IDamageable>().OnHit(attackDamage);
        }
    }

    void AttackFinished(){
        isAttacking = false;
    }
    void TriggerSound(){
        attackSound.Play();
    }

    public void OnHit(float damage){
        if (isDead) return; // cant do anythin if im dead
        if (isInvincible) return; // cant do anythin if im dead
        hp-=damage;

        hitSound.Play();
        
        animator.SetTrigger("doHit");

        isRecoiling = true;
        isAttacking = false; //cancel attack
        if (hp <= 0){
            Die();
            screechSound.clip = deathScreech;
        } else {
            screechSound.clip = screeches[Random.Range(0,screeches.Length)];
        }
        screechSound.pitch = Random.Range(0.8f, 1.1f);
        screechSound.Play();
    }
    void Die(){
        animator.SetTrigger("doDie");
        //animator.SetBool("isDead", true);
        isDead = true;
        walkSound.Stop();
        // remove collision of dead enemies
        this.gameObject.GetComponent<CharacterController>().enabled = false;
    }

    void HitFinished(){
        isRecoiling = false;
    }

}
