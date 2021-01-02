using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,

        RunningToEnemy,
        RunningFromEnemy,

        BeginCloseAttack,
        CloseAttack,

        BeginShoot,
        Shoot,

        Dies,
        Dead
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Unarmed
    }

    Animator animator;
    State state;

    public Weapon weapon;
    public Transform target;
    public float runSpeed;
    public float distanceFromEnemy;
    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void TargetDies()
    {
        target.gameObject.GetComponent<Character>().SetState( Character.State.Dies );
    }

    public bool isDead(){
        return false;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    [ContextMenu("Attack")]
    void AttackTarget()
    {
        if( CanAttack() ){
            switch (weapon) {
                case Weapon.Unarmed:
                    state = State.RunningToEnemy;
                    break;
                case Weapon.Bat:
                    state = State.RunningToEnemy;
                    break;
                case Weapon.Pistol:
                    state = State.BeginShoot;
                    break;
            }
        }
    }

    [ContextMenu("Attack", isValidateFunction:true)]
    bool CanAttack()
    {
        return IsAlive() && IsTargetAlive();
    }

    public bool IsAlive()
    {
        return state != State.Dead;
    }

    public bool IsTargetAlive()
    {
        return target.gameObject.GetComponent<Character>().IsAlive();
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.position, distanceFromEnemy))
                    state = State.BeginCloseAttack;
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginCloseAttack:
                switch (weapon) {
                    case Weapon.Unarmed:
                        animator.SetTrigger("UnarmedAttack");
                        break;
                    case Weapon.Bat:
                        animator.SetTrigger("MeleeAttack");
                        break;
                }
                state = State.CloseAttack;
                break;

            case State.CloseAttack:
                TargetDies();
                break;

            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                TargetDies();
                break;

            case State.Dies:
                animator.SetBool("Dead", true);
                state = State.Dead;
                break;

            case State.Dead:
                break;
        }
    }
    
    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        //Debug.Log("targetPosition: " + targetPosition);
        //Debug.Log("transform.position: " + transform.position);

        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            //Debug.Log("teleport");
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }
}
