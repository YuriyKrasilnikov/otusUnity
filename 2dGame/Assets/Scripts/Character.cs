using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

using System.Linq;

public class Character : MonoBehaviour
{
    public GameObject LevelZone;
    public GameObject Menu;
    public Transform Visual;
    public float MoveForce;
    public float JumpForce;
    public float JumpForceHorizontal;

    public CinemachineVirtualCamera CoinCamera;

    Rigidbody2D rigidBody2D;
    TriggerDetector triggerDetector;
    Animator animator;
    float visualDirection;

   

    // Start is called before the first frame update
    void Start()
    {
        visualDirection = 1.0f;
        rigidBody2D = GetComponent<Rigidbody2D>();
        triggerDetector = GetComponentInChildren<TriggerDetector>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Move(float force)
    {   
        if (triggerDetector.InTrigger)
        {
            //rigidBody2D.AddForce(new Vector2(MoveForce * force, 0), ForceMode2D.Force);
            rigidBody2D.velocity = new Vector2(MoveForce * force, rigidBody2D.velocity.y);
        }

    }

    private void Jump()
    {
        if (triggerDetector.InTrigger) {
            rigidBody2D.AddForce(new Vector2(rigidBody2D.velocity.x*JumpForceHorizontal, JumpForce), ForceMode2D.Impulse);
            rigidBody2D.rotation = 0;
        }
    }

    private void MoveAnimate()
    {
        float vel = rigidBody2D.velocity.x;

        if (vel < -0.01f)
            visualDirection = -1.0f;
        else if (vel > 0.01f)
            visualDirection = 1.0f;

        Vector3 scale = Visual.localScale;
        scale.x = visualDirection;
        Visual.localScale = scale;

        animator.SetFloat("speed", Mathf.Abs(vel));
        animator.SetBool("onGround", triggerDetector.InTrigger);
    }
     
    private void Update()
    {
        Move(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //Debug.Log(rigidBody2D.velocity);

        if(rigidBody2D.rotation != 0)
        {
            rigidBody2D.AddTorque(-rigidBody2D.rotation/25);
        }

        MoveAnimate();

    }

    void OnTriggerExit2D(Collider2D colliderInfo) {
        //Debug.Log("No longer in contact with " + colliderInfo.transform.name);

        if (colliderInfo.gameObject == LevelZone) {
            Debug.Log("Dead!!!");
            Menu.SetActive(true);
            Destroy(gameObject);
        }
    }

}
