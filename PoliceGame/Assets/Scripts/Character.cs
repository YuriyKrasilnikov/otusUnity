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
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        BeginDeath,
        Death
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist
    }

    public float runSpeed;
    public float distanceFromEnemy;
    public Character target;
    public Weapon weapon;
    public float damage;
    Animator animator;
    Vector3 originalPosition;
    Quaternion originalRotation;
    State state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void AttackEnemy()
    {
        if (state != State.Death && target.state != State.Death) {
            switch (weapon)
            {
                case Weapon.Bat:
                    state = State.RunningToEnemy;
                    break;

                case Weapon.Fist:
                    state = State.RunningToEnemy;
                    break;

                case Weapon.Pistol:
                    state = State.BeginShoot;
                    break;
            }
        }
    }

    public bool IsIdle()
    {
        return state == State.Idle;
    }

    public bool IsDead()
    {
        return state == State.Death;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    public void TargetSetState(State newState)
    {
        target.SetState(newState);
    }

    public void DoDamageToTarget()
    {
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.ApplyDamage(damage);
            if (health.current <= 0.0f)
            {
                TargetSetState(State.BeginDeath);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state){
            case State.Idle:
                animator.SetFloat("speed", 0.0f);
                transform.rotation = originalRotation;
                break;

            case State.RunningToEnemy:
                animator.SetFloat("speed", runSpeed);
                if (RunTowards(target.transform.position, distanceFromEnemy))
                {
                    state = State.BeginAttack;
                }
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                {
                    state = State.Idle;
                }
                break;

            case State.BeginAttack:
                animator.SetFloat("speed", 0.0f);
                switch (weapon)
                {
                    case Weapon.Bat:
                        animator.SetTrigger("attack");
                        state = State.Attack;
                        break;

                    case Weapon.Fist:
                        animator.SetTrigger("punch");
                        state = State.Attack;
                        break;
                }
                
                
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger("shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                break;

            case State.BeginDeath:
                Debug.Log(gameObject.name + " dead!");
                animator.SetTrigger("killed");
                state = State.Death;
                break;
        }

    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        Vector3 direction = distance.normalized;

        transform.rotation = Quaternion.LookRotation(direction);

        //Debug.Log($"{distance.magnitude}");

        targetPosition -= direction * distanceFromTarget;
        distance = targetPosition - transform.position;

        Vector3 vector = direction * runSpeed;
        if (vector.magnitude < distance.magnitude)
        {
            transform.position += vector;
            return false;
        }

        transform.position = targetPosition;
        return true;

    }
}
