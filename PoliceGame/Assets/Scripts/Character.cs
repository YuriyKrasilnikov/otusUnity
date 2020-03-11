using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private float damage = 1.0f;

    private Animator animator;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private State state = State.Idle;
    private SoundPlayerСontrol selfSoundPlayerСontrol;

    [HideInInspector]
    public ParticleSystem shootFire;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        selfSoundPlayerСontrol = GetComponent<SoundPlayerСontrol>();
        damage = GetComponent<Balance>().characterBalanceData.Damage;
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
                    shootFire.Play();
                    selfSoundPlayerСontrol.Shoot();
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
        switch (weapon)
        {
            case Weapon.Bat:
                selfSoundPlayerСontrol.BatHit();
                break;

            case Weapon.Fist:
                selfSoundPlayerСontrol.FistHit();
                break;
        }

        HitEffectAnimation hitEffect = target.GetComponent<HitEffectAnimation>();

        SoundPlayerСontrol targetSoundPlayerСontrol = target.GetComponent<SoundPlayerСontrol>();

        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.ApplyDamage(damage);
            if (health.Current <= 0.0f)
            {
                TargetSetState(State.BeginDeath);
                targetSoundPlayerСontrol.Dead();
            }
            else
            {
                targetSoundPlayerСontrol.Hit();
            }
            hitEffect.PlayEffect();
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
                animator.SetTrigger("pistol_idle");
                state = State.Shoot;
                break;

            case State.Shoot:
                animator.SetTrigger("shoot");
                break;

            case State.BeginDeath:
                //Debug.Log(gameObject.name + " dead!");
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
            selfSoundPlayerСontrol.StartRun();
            return false;
        }

        transform.position = targetPosition;
        selfSoundPlayerСontrol.StopRun();
        return true;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Character))]
public class Character_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //The target variable is the selected script.
        Character mb = (Character)target;

        if (mb.weapon == Character.Weapon.Pistol)
        {
            mb.shootFire = EditorGUILayout.ObjectField("Shoot Effect", mb.shootFire, typeof(ParticleSystem), true) as ParticleSystem;
        }
    }
}
#endif