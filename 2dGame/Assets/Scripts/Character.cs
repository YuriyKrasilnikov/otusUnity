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
    public Transform Visual;
    public float MoveForce;
    public float JumpForce;
    public float JumpForceHorizontal;

    Rigidbody2D rigidBody2D;
    TriggerDetector triggerDetector;
    Animator animator;
    float visualDirection;

    private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
    private static readonly int AnimatorOnGround = Animator.StringToHash("onGround");


    // Start is called before the first frame update
    void Awake()
    {
        visualDirection = 1.0f;
        rigidBody2D = GetComponent<Rigidbody2D>();
        triggerDetector = GetComponentInChildren<TriggerDetector>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(float force)
    {
        if (triggerDetector.InTrigger)
        {
            //rigidBody2D.AddForce(new Vector2(MoveForce * force, 0), ForceMode2D.Force);
            rigidBody2D.velocity = new Vector2(MoveForce * force, rigidBody2D.velocity.y);
        }
    }

    public void Jump()
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

        animator.SetFloat(AnimatorSpeed, Mathf.Abs(vel));
        animator.SetBool(AnimatorOnGround, triggerDetector.InTrigger);
    }
     
    void Update()
    {

        if (rigidBody2D.rotation != 0)
        {
            rigidBody2D.AddTorque(-rigidBody2D.rotation / 25);
        }

        MoveAnimate();

    }

    void OnTriggerExit2D(Collider2D colliderInfo) {
        if (colliderInfo.gameObject == LevelZone) {
            Destroy(gameObject);
        }
    }

}
