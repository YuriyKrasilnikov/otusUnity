using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SFXEventListener: SingletonMonoBehaviour<SFXEventListener>
{

    public AudioClip[] TakeDamageSound;

    public AudioClip[] DeadSound;

    public AudioClip[] StepSound;

    public AudioClip[] ShootSound;
    public AudioClip[] BatSound;
    public AudioClip[] FistSound;

    public AudioClip[] ClickSound;
    public AudioClip[] WonSound;
    public AudioClip[] LostSound;

    private AudioSource stepAudioSource = null;

    private Button[] buttons;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        OnInit();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnInit()
    {
        EventManager.Instance.Sub(EventId.UnitDamage, this.OnUnitDamage);
        EventManager.Instance.Sub(EventId.UnitDead, this.OnUnitDead);

        EventManager.Instance.Sub(EventId.UnitStartStep, this.OnStartUnitStep);
        EventManager.Instance.Sub(EventId.UnitStopStep, this.OnStoptUnitStep);

        EventManager.Instance.Sub(EventId.UnitShoot, this.OnUnitShoot);
        EventManager.Instance.Sub(EventId.UnitBatHit, this.OnUnitBatHit);
        EventManager.Instance.Sub(EventId.UnitFistHit, this.OnUnitFistHit);

        EventManager.Instance.Sub(EventId.PlayerWin, this.OnWin);
        EventManager.Instance.Sub(EventId.PlayerLose, this.OnLose);

        buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(this.OnButtonClick);
        }
    }

    private void OnUnSub()
    {
        EventManager.Instance.Unsub(EventId.UnitDamage, this.OnUnitDamage);
        EventManager.Instance.Unsub(EventId.UnitDead, this.OnUnitDead);

        EventManager.Instance.Unsub(EventId.UnitStartStep, this.OnStartUnitStep);
        EventManager.Instance.Unsub(EventId.UnitStopStep, this.OnStoptUnitStep);

        EventManager.Instance.Unsub(EventId.UnitShoot, this.OnUnitShoot);
        EventManager.Instance.Unsub(EventId.UnitBatHit, this.OnUnitBatHit);
        EventManager.Instance.Unsub(EventId.UnitFistHit, this.OnUnitFistHit);

        EventManager.Instance.Unsub(EventId.PlayerWin, this.OnWin);
        EventManager.Instance.Unsub(EventId.PlayerLose, this.OnLose);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //OnUnSub();
        //OnInit();
    }

    protected override void OnDestroy()
    {
        OnUnSub();
        base.OnDestroy();
    }

    private void OnUnitDamage()
    {

        SFXManager.Instance.Play(TakeDamageSound, transform.position);

        //Debug.Log("OnHit sound");
    }
    private void OnStartUnitStep()
    {
        if (stepAudioSource == null || !stepAudioSource.isPlaying) {
            stepAudioSource = SFXManager.Instance.Play(StepSound, transform.position);
        }
        //Debug.Log("Step sound");
    }

    private void OnStoptUnitStep()
    {

        if (stepAudioSource != null)
        {
            stepAudioSource.Stop();
            stepAudioSource = null;
        }
        //Debug.Log("Stop Step sound");
    }
    private void OnUnitShoot()
    {
        SFXManager.Instance.Play(ShootSound, transform.position);
    }

    private void OnUnitBatHit()
    {
        SFXManager.Instance.Play(BatSound, transform.position);
    }

    private void OnUnitFistHit()
    {
        SFXManager.Instance.Play(FistSound, transform.position);
    }

    private void OnUnitDead()
    {
        SFXManager.Instance.Play(DeadSound, transform.position);

        //Debug.Log("OnUnitDead sound");
    }

    private void OnButtonClick()
    {
        SFXManager.Instance.Play(ClickSound, transform.position);

    }

    private void OnWin()
    {
        SFXManager.Instance.Play(WonSound, transform.position);
        //Debug.Log("OnWin sound");
    }
    private void OnLose()
    {
        SFXManager.Instance.Play(LostSound, transform.position);
        //Debug.Log("OnLose sound");
    }

}
