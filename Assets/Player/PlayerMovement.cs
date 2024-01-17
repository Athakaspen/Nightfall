using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    
    public float moveSpeed = 12f;
    public float sprintMult = 1.8f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundMargin = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    public bool isSprinting;
    public bool isMoving;

    float initialStepOffset;
    // grabbed in start function, used because we set it to zero in midair

    void Start(){
        initialStepOffset = controller.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // update states
        isGrounded = Physics.CheckSphere(groundCheck.position, groundMargin, groundMask);
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // cancel y velocity on the ground
        if (isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        // disable step offset in midair (fixes glitching around ledges)
        if(isGrounded){
            controller.stepOffset = initialStepOffset;
        } else {
            controller.stepOffset = 0f;
        }

        // input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // move from input
        Vector3 move = transform.right * x + transform.forward * z; 
        controller.Move(move * moveSpeed * (isSprinting?sprintMult:1f) * Time.deltaTime);
        isMoving = move.magnitude > 0.5f;

        // jump
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // gravity
        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
    }
}
