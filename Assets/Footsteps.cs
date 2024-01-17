using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
public class Footsteps : MonoBehaviour
{
    CharacterController cc;
    PlayerMovement pm;
    public AudioSource source;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<PlayerMovement>();
        //source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded && pm.isMoving && !source.isPlaying){
            if (pm.isSprinting)
                source.pitch = 1.5f;
            else
                source.pitch = 1f;
            source.clip = clips[Random.Range(0,clips.Length)];
            source.Play();
        }
    }
}
